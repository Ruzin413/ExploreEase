$(document).ready(function () {
    const $blogsContainer = $('#blogsContainer');
    const $commentModal = $('#commentModal');
    const $commentsList = $('#commentsList');
    const $commentForm = $('#commentForm');
    const $commentBlogId = $('#commentBlogId');
    const $blogForm = $('#blogForm');
    let blogToDeleteId = null;

    function formatRelativeTime(dateString) {
        const date = new Date(dateString);
        const now = new Date();
        const seconds = Math.floor((now - date) / 1000);

        let interval = Math.floor(seconds / 31536000);
        if (interval >= 1) return `${interval}y`;
        interval = Math.floor(seconds / 2592000);
        if (interval >= 1) return `${interval}mo`;
        interval = Math.floor(seconds / 86400);
        if (interval >= 1) return `${interval}d`;
        interval = Math.floor(seconds / 3600);
        if (interval >= 1) return `${interval}h`;
        interval = Math.floor(seconds / 60);
        if (interval >= 1) return `${interval}m`;
        return 'Just now';
    }

    function escapeHtml(text) {
        return $('<div>').text(text).html();
    }

    function updateLikesCount($element, count) {
        count = parseInt(count) || 0;
        $element.text(count + (count === 1 ? ' like' : ' likes')).data('count', count);
    }

    function showNotification(message, type = 'info') {
        const $notification = $(`<div class="notification ${type}">${message}</div>`);
        $('body').append($notification);

        setTimeout(() => {
            $notification.addClass('show');
            setTimeout(() => {
                $notification.removeClass('show');
                setTimeout(() => $notification.remove(), 300);
            }, 3000);
        }, 100);
    }

    function loadComments(blogId) {
        $.get('/Home/GetComments', { BlogId: blogId }, function (comments) {
            $commentsList.empty();
            if (comments.length === 0) {
                $commentsList.html('<div class="no-comments">No comments yet. Be the first to comment!</div>');
                return;
            }

            comments.forEach(comment => {
                const commentHtml = `
                    <div class="comment-item">
                        <div class="comment-content">
                            <h4 class="comment-username">${escapeHtml(comment.name || 'User')}</h4>
                            <p class="comment-text">${escapeHtml(comment.comment || '')}</p>
                        </div>
                    </div>
                `;
                $commentsList.append(commentHtml);
            });
        }).fail(() => {
            $commentsList.html('<div class="error-message">Failed to load comments. Please try again.</div>');
        });
    }

    function loadBlogs() {
        $.ajax({
            url: '/Home/GetBlog',
            method: 'GET',
            success: function (blogs) {
                $blogsContainer.empty();
                if (blogs.length === 0) {
                    $blogsContainer.html(`
                        <div class="empty-state">
                            <i class="fas fa-camera" style="font-size: 48px; margin-bottom: 16px; opacity: 0.3;"></i>
                            <h3>No Posts Yet</h3>
                            <p>Share your first photo to get started!</p>
                        </div>
                    `);
                    return;
                }

                blogs.forEach(blog => {
                    const isLiked = blog.likestatus === true || blog.likestatus === 'true';
                    const likeIcon = isLiked ? 'fa-solid fa-heart' : 'fa-regular fa-heart';
                    const likeClass = isLiked ? 'liked' : '';
                    const likeCount = parseInt(blog.likes) || 0;
                    const commentCount = blog.commentCount || 0;
                    const postTime = formatRelativeTime(blog.createdAt || new Date().toISOString());

                    const isOwner = blog.email === window.currentUser.email;
                    const canDelete = isOwner || window.currentUser.isAdmin;
                    const deleteButtonHtml = canDelete
                        ? `<button type="button" class="delete-action" data-blogid="${blog.id}" style="position:absolute; top:10px; right:10px; background: rgba(255,255,255,0.8); border:none; border-radius:50%; padding:6px; cursor:pointer;">
           <i class="fa-regular fa-trash-can"></i>
       </button>`
                        : '';

                    const blogHtml = `
    <article class="blog-post" data-blogid="${blog.id}" style="position:relative;">
        ${deleteButtonHtml}
        <div class="post-header">
            <img src="/images/default-avatar.png" alt="Profile" class="post-avatar">
            <h3 class="post-username">${escapeHtml(blog.name || 'User')}</h3>
        </div>
        <div class="post-image-container">
            <img src="${escapeHtml(blog.blogimage || '')}" alt="Post" class="post-image"
                 onerror="this.src='/images/placeholder-image.jpg'">
        </div>
        <div class="post-actions">
            <div class="action-icons">
                <button type="button" class="action-icon like-action ${likeClass}" data-blogid="${blog.id}">
                    <i class="${likeIcon}"></i>
                </button>
                <button type="button" class="action-icon comment-action" data-blogid="${blog.id}">
                    <i class="fa-regular fa-comment"></i>
                </button>
            </div>
            <div class="likes-count">${likeCount} ${likeCount === 1 ? 'like' : 'likes'}</div>
            <p class="post-caption">
                <strong>${escapeHtml(blog.name || 'User')}</strong> ${escapeHtml(blog.description || '')}
            </p>
            ${commentCount > 0 ? `<div class="post-comments view-comments" data-blogid="${blog.id}">
                View all ${commentCount} ${commentCount === 1 ? 'comment' : 'comments'}
            </div>` : ''}
            <div class="post-time">${postTime}</div>
        </div>
    </article>
`;
                    $blogsContainer.append(blogHtml);
                });
            },
            error: function () {
                $blogsContainer.html('<div class="error-message">Failed to load posts. Please try again later.</div>');
            }
        });
    }

    // New Post Form
    $blogForm.on('submit', function (e) {
        e.preventDefault();
        const $submitBtn = $(this).find('button[type="submit"]');
        const originalBtnText = $submitBtn.text();
        $submitBtn.text('Sharing...').prop('disabled', true);

        let formData = new FormData(this);

        $.ajax({
            url: "/Home/PostBlog",
            method: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                $submitBtn.text('Shared!');
                $blogForm[0].reset();
                document.getElementById('modal').close();
                loadBlogs();
                setTimeout(() => {
                    $submitBtn.text(originalBtnText).prop('disabled', false);
                }, 2000);
            },
            error: function () {
                $submitBtn.text('Error!').css('background-color', '#ff4d4d');
                setTimeout(() => {
                    $submitBtn.text(originalBtnText).css('background-color', '').prop('disabled', false);
                }, 2000);
            }
        });
    });

    // Like/Unlike Post
    $blogsContainer.on('click', '.like-action', function (e) {
        e.preventDefault();
        e.stopPropagation();

        const $action = $(this);
        const blogId = $action.data('blogid');
        const $post = $action.closest('.blog-post');
        const $heartIcon = $action.find('i');
        const $likesCount = $post.find('.likes-count');

        let currentLikes = parseInt($likesCount.data('count') || $likesCount.text().match(/\d+/) || 0);
        const isLiked = $action.hasClass('liked');

        if (!isLiked) {
            currentLikes++;
            $action.addClass('liked');
            $heartIcon.removeClass('fa-regular').addClass('fa-solid');
            updateLikesCount($likesCount, currentLikes);

            $.post('/Home/BlogLike', { blogId }).fail(() => {
                currentLikes--;
                $action.removeClass('liked');
                $heartIcon.removeClass('fa-solid').addClass('fa-regular');
                updateLikesCount($likesCount, currentLikes);
                showNotification('Failed to like post', 'error');
            });
        } else {
            currentLikes = Math.max(0, currentLikes - 1);
            $action.removeClass('liked');
            $heartIcon.removeClass('fa-solid').addClass('fa-regular');
            updateLikesCount($likesCount, currentLikes);

            $.post('/Home/BlogUnlike', { blogId }).fail(() => {
                currentLikes++;
                $action.addClass('liked');
                $heartIcon.removeClass('fa-regular').addClass('fa-solid');
                updateLikesCount($likesCount, currentLikes);
                showNotification('Failed to unlike post', 'error');
            });
        }
    });

    // Double Tap to Like
    $blogsContainer.on('dblclick', '.post-image-container', function (e) {
        e.preventDefault();
        e.stopPropagation();

        const $post = $(this).closest('.blog-post');
        const $likeButton = $post.find('.like-action');

        if (!$likeButton.hasClass('liked')) {
            $likeButton.trigger('click');
            const $heart = $('<div class="heart-animation"><i class="fas fa-heart"></i></div>');
            $(this).append($heart);
            setTimeout(() => {
                $heart.fadeOut(300, function () { $(this).remove(); });
            }, 1000);
        }
    });

    // Comment Button
    $blogsContainer.on('click', '.comment-action, .view-comments', function () {
        const blogId = $(this).data('blogid');
        $commentBlogId.val(blogId);
        $commentsList.html('<div class="loading-spinner"><i class="fa fa-spinner fa-spin"></i></div>');
        loadComments(blogId);
        $commentModal[0].showModal();
    });

    // Submit Comment
    $commentForm.on('submit', function (e) {
        e.preventDefault();
        const data = $(this).serialize();

        $.post('/Home/PostComment', data)
            .done(() => {
                loadComments($commentBlogId.val());
                $commentForm[0].reset();
            })
            .fail(() => {
                alert('Error submitting comment');
            });
    });

    // Delete Post
    $blogsContainer.on('click', '.delete-action', function () {
        blogToDeleteId = $(this).data('blogid');
        document.getElementById('deleteModal').showModal();
    });

    $('#confirmDeleteBtn').on('click', function () {
        if (!blogToDeleteId) return;

        $.post('/Home/DeleteBlog', { blogId: blogToDeleteId })
            .done(() => {
                $(`.blog-post[data-blogid="${blogToDeleteId}"]`).remove();
                document.getElementById('deleteModal').close();
                showNotification('Post deleted successfully', 'success');
            })
            .fail(() => {
                showNotification('Failed to delete post', 'error');
            });
    });

    // Initial Load
    loadBlogs();
});

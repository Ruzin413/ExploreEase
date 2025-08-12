$(document).ready(function () {
    const $blogsContainer = $('#blogsContainer');
    const $commentModal = $('#commentModal');
    const $commentsList = $('#commentsList');
    const $commentForm = $('#commentForm');
    const $commentBlogId = $('#commentBlogId');
    const $blogForm = $('#blogForm');

    // Format date to relative time (e.g., "2 hours ago")
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

    // Submit new post
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
            success: function (res) {
                $submitBtn.text('Shared!');
                $blogForm[0].reset();
                document.getElementById('modal').close();
                loadBlogs();
                
                // Reset button after delay
                setTimeout(() => {
                    $submitBtn.text(originalBtnText).prop('disabled', false);
                }, 2000);
            },
            error: function (xhr) {
                $submitBtn.text('Error!').css('background-color', '#ff4d4d');
                setTimeout(() => {
                    $submitBtn.text(originalBtnText).css('background-color', '').prop('disabled', false);
                }, 2000);
            }
        });
    });

    // Load blogs from server and render
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
                    console.log('Blog data:', blog); // Debug log
                    const isLiked = blog.likestatus === true || blog.likestatus === 'true';
                    const likeIcon = isLiked ? 'fa-solid fa-heart' : 'fa-regular fa-heart';
                    const likeClass = isLiked ? 'liked' : '';
                    const likeCount = parseInt(blog.likes) || 0; // Ensure we parse as integer
                    const commentCount = blog.commentCount || 0;
                    const postTime = formatRelativeTime(blog.createdAt || new Date().toISOString());
                    
                    const blogHtml = `
                        <article class="blog-post" data-blogid="${blog.id}">
                            <div class="post-header">
                                <img src="/images/default-avatar.png" alt="Profile" class="post-avatar">
                                <h3 class="post-username">${escapeHtml(blog.name || 'User')}</h3>
                            </div>
                            <div class="post-image-container">
                                <img src="${escapeHtml(blog.blogimage || '')}" alt="Post" class="post-image" onerror="this.src='/images/placeholder-image.jpg'">
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
                                ${commentCount > 0 ? `<div class="post-comments view-comments" data-blogid="${blog.id}">View all ${commentCount} ${commentCount === 1 ? 'comment' : 'comments'}</div>` : ''}
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

    // Escape HTML utility (basic)
    function escapeHtml(text) {
        return $('<div>').text(text).html();
    }

    // Like/Unlike post
    $blogsContainer.on('click', '.like-action', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const $action = $(this);
        const blogId = $action.data('blogid');
        const $post = $action.closest('.blog-post');
        const $heartIcon = $action.find('i');
        const $likesCount = $post.find('.likes-count');
        
        // Get current like count from data attribute if available, otherwise from text
        let currentLikes = parseInt($likesCount.data('count') || $likesCount.text().match(/\d+/) || 0);
        const isLiked = $action.hasClass('liked');
        
        console.log('Like button clicked. Current state:', { 
            isLiked, 
            currentLikes,
            text: $likesCount.text(),
            dataCount: $likesCount.data('count')
        });
        
        // Toggle like state immediately for better UX
        $action.toggleClass('liked');
        
        if (!isLiked) {
            // Like action
            currentLikes++;
            $heartIcon.removeClass('fa-regular').addClass('fa-solid');
            
            // Animate like
            $action.addClass('pulse');
            setTimeout(() => $action.removeClass('pulse'), 300);
            
            // Update UI immediately
            updateLikesCount($likesCount, currentLikes);
            
            // Call like API
            $.ajax({
                url: '/Home/BlogLike',
                method: 'POST',
                data: { blogId: blogId },
                success: function(response) {
                    console.log('Like successful', response);
                    if (response && response.success) {
                        // Update with server count
                        if (typeof response.likes !== 'undefined') {
                            currentLikes = parseInt(response.likes);
                            updateLikesCount($likesCount, currentLikes);
                        }
                        // Update like status
                        $action.toggleClass('liked', true);
                        $heartIcon.removeClass('fa-regular').addClass('fa-solid');
                    } else {
                        // Revert on failure
                        currentLikes--;
                        $action.removeClass('liked');
                        $heartIcon.removeClass('fa-solid').addClass('fa-regular');
                        updateLikesCount($likesCount, currentLikes);
                        showNotification('Failed to like post', 'error');
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error liking post:', error);
                    // Revert on error
                    currentLikes--;
                    $action.removeClass('liked');
                    $heartIcon.removeClass('fa-solid').addClass('fa-regular');
                    updateLikesCount($likesCount, currentLikes);
                    showNotification('Failed to like post', 'error');
                }
            });
        } else {
            // Unlike action
            currentLikes = Math.max(0, currentLikes - 1);
            $heartIcon.removeClass('fa-solid').addClass('fa-regular');
            
            // Update UI immediately
            updateLikesCount($likesCount, currentLikes);
            
            // Call unlike API
            $.ajax({
                url: '/Home/BlogUnlike',
                method: 'POST',
                data: { blogId: blogId },
                success: function(response) {
                    console.log('Unlike successful', response);
                    if (response && response.success) {
                        // Update with server count
                        if (typeof response.likes !== 'undefined') {
                            currentLikes = parseInt(response.likes);
                            updateLikesCount($likesCount, currentLikes);
                        }
                        // Update like status
                        $action.toggleClass('liked', false);
                        $heartIcon.removeClass('fa-solid').addClass('fa-regular');
                    } else {
                        // Revert on failure
                        currentLikes++;
                        $action.addClass('liked');
                        $heartIcon.removeClass('fa-regular').addClass('fa-solid');
                        updateLikesCount($likesCount, currentLikes);
                        showNotification('Failed to unlike post', 'error');
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Error unliking post:', error);
                    // Revert on error
                    currentLikes++;
                    $action.addClass('liked');
                    $heartIcon.removeClass('fa-regular').addClass('fa-solid');
                    updateLikesCount($likesCount, currentLikes);
                    showNotification('Failed to unlike post', 'error');
                }
            });
        }
    });
    
    // Helper function to update likes count
    function updateLikesCount($element, count) {
        count = parseInt(count) || 0;
        $element
            .text(count + (count === 1 ? ' like' : ' likes'))
            .data('count', count);
        console.log('Updating likes count:', count, 'for element:', $element);
    }

    // Double tap to like
    $blogsContainer.on('dblclick', '.post-image-container', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const $post = $(this).closest('.blog-post');
        const $likeButton = $post.find('.like-action');
        
        // Only trigger like if not already liked
        if (!$likeButton.hasClass('liked')) {
            $likeButton.trigger('click');
            
            // Show heart animation
            const $heart = $('<div class="heart-animation"><i class="fas fa-heart"></i></div>');
            $(this).append($heart);
            
            // Remove after animation
            setTimeout(() => {
                $heart.fadeOut(300, function() {
                    $(this).remove();
                });
            }, 1000);
        }
    });

    // Comment button click
    $blogsContainer.on('click', '.comment-action, .view-comments', function() {
        const blogId = $(this).data('blogid');
        $commentBlogId.val(blogId);
        $commentsList.html('<div class="loading-spinner"><i class="fa fa-spinner fa-spin"></i></div>');
        loadComments(blogId);
        $commentModal[0].showModal();
    });

    // Load comments for a blog
    function loadComments(blogId) {
        $.ajax({
            url: '/Home/GetComments',
            method: 'GET',
            data: { BlogId: blogId },
            success: function(comments) {
                $commentsList.empty();
                
                if (comments.length === 0) {
                    $commentsList.html('<div class="no-comments">No comments yet. Be the first to comment!</div>');
                    return;
                }
                
                comments.forEach(comment => {
                    const commentTime = formatRelativeTime(comment.createdAt || new Date().toISOString());
                    const commentHtml = `
                        <div class="comment-item">
                            <div class="comment-content">
                                <div style="display: flex; justify-content: space-between; align-items: center;">
                                    <h4 class="comment-username">${escapeHtml(comment.name || 'User')}</h4>
                                </div>
                                <p class="comment-text">${escapeHtml(comment.comment || '')}</p>
                            </div>
                        </div>
                    `;
                    $commentsList.append(commentHtml);
                });
            },
            error: function() {
                $commentsList.html('<div class="error-message">Failed to load comments. Please try again.</div>');
            }
        });
    }
    
    // Show notification
    function showNotification(message, type = 'info') {
        const $notification = $(`
            <div class="notification ${type}">
                ${message}
            </div>
        `);
        
        $('body').append($notification);
        setTimeout(() => {
            $notification.addClass('show');
            setTimeout(() => {
                $notification.removeClass('show');
                setTimeout(() => $notification.remove(), 300);
            }, 3000);
        }, 100);
    }

    // Submit comment form
    $commentForm.on('submit', function (e) {
        e.preventDefault();
        const data = $(this).serialize();
        $.ajax({
            url: '/Home/PostComment',
            method: 'POST',
            data: data,
            success: function () {
                loadComments($commentBlogId.val());
                $commentForm[0].reset();
            },
            error: function () {
                alert('Error submitting comment');
            }
        });
    });

    // Initial load
    loadBlogs();
});
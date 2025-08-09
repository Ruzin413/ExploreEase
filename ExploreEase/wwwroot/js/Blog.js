$(document).ready(function () {
    $("#blogForm").on("submit", function (e) {
        e.preventDefault();
        let formData = new FormData(this);
        $.ajax({
            url: "/Home/PostBlog",
            method: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                $('#submit').text("Done");
                $("#blogForm")[0].reset();
            },
            error: function (xhr) {
                $('#submit').text("Error");
            }
        });
    });
});
$(document).ready(function () {
    const $blogsContainer = $('#blogsContainer');
    const $commentModal = $('#commentModal');
    const $commentsList = $('#commentsList');
    const $commentForm = $('#commentForm');
    const $commentBlogId = $('#commentBlogId');

    // Load blogs from server and render
    function loadBlogs() {
        $.ajax({
            url: '/Home/GetBlog',
            method: 'GET',
            success: function (blogs) {
                console.log(blogs); // for debugging
                $blogsContainer.empty();

                blogs.forEach(blog => {
                    let likeButtonHtml = '';
                    if (blog.likestatus === true) {
                        likeButtonHtml = `<button class="unlikeBtn">Unlike (${blog.likes || 0})</button>`;
                    } else {
                        likeButtonHtml = `<button class="likeBtn">Like (${blog.likes || 0})</button>`;
                    }

                    const blogHtml = `
                        <div class="blog" data-blogid="${blog.id}" style="border:1px solid #ccc; padding:10px; margin-bottom:10px;">
                          <h4>${escapeHtml(blog.name)}</h4>
                          <p>${escapeHtml(blog.description)}</p>
                          ${blog.blogimage ? `<img src="${escapeHtml(blog.blogimage)}" alt="Blog Image" style="max-width:100%; max-height:200px;"/>` : ''}
                          <div>
                            ${likeButtonHtml}
                            <button class="commentBtn">Comment</button>
                          </div>
                        </div>
                    `;
                    $blogsContainer.append(blogHtml);
                });
            },
            error: function () {
                alert('Failed to load blogs.');
            }
        });
    }

    // Escape HTML utility (basic)
    function escapeHtml(text) {
        return $('<div>').text(text).html();
    }

    // Like button click
    $blogsContainer.on('click', '.likeBtn', function () {
        const $blogDiv = $(this).closest('.blog');
        const blogId = $blogDiv.data('blogid');
        const $likeBtn = $(this);
        const $unlikeBtn = $blogDiv.find('.unlikeBtn');

        $.ajax({
            url: '/Home/BlogLike',
            method: 'POST',
            data: { blogId: blogId },
            success: function (res) {
                if (res.success) {
                    // Increment likes count shown
                    const currentText = $likeBtn.text();
                    const match = currentText.match(/\d+/);
                    let likes = match ? parseInt(match[0]) : 0;
                    likes++;
                    $likeBtn.text(`Like (${likes})`);
                    $likeBtn.hide();
                    if ($unlikeBtn.length) {
                        $unlikeBtn.text(`Unlike (${likes})`);
                        $unlikeBtn.show();
                    } else {
                        // If unlikeBtn not present, add it
                        $blogDiv.find('div').prepend(`<button class="unlikeBtn">Unlike (${likes})</button>`);
                    }
                } else {
                    alert(res.message || 'Could not like blog');
                }
            },
            error: function () {
                alert('Error liking blog');
            }
        });
    });

    // Unlike button click
    $blogsContainer.on('click', '.unlikeBtn', function () {
        const $blogDiv = $(this).closest('.blog');
        const blogId = $blogDiv.data('blogid');
        const $unlikeBtn = $(this);
        const $likeBtn = $blogDiv.find('.likeBtn');

        $.ajax({
            url: '/Home/BlogUnlike',
            method: 'POST',
            data: { blogId: blogId },
            success: function (res) {
                if (res.success) {
                    // Decrement likes count shown
                    const currentText = $unlikeBtn.text();
                    const match = currentText.match(/\d+/);
                    let likes = match ? parseInt(match[0]) : 0;
                    likes = Math.max(likes - 1, 0);
                    $unlikeBtn.text(`Unlike (${likes})`);
                    $unlikeBtn.hide();
                    if ($likeBtn.length) {
                        $likeBtn.text(`Like (${likes})`);
                        $likeBtn.show();
                    } else {
                        // If likeBtn not present, add it
                        $blogDiv.find('div').prepend(`<button class="likeBtn">Like (${likes})</button>`);
                    }
                } else {
                    alert(res.message || 'Could not unlike blog');
                }
            },
            error: function () {
                alert('Error unliking blog');
            }
        });
    });

    // Comment button click: open modal and load comments
    $blogsContainer.on('click', '.commentBtn', function () {
        const $blogDiv = $(this).closest('.blog');
        const blogId = $blogDiv.data('blogid');
        $commentBlogId.val(blogId);
        $commentsList.empty();
        loadComments(blogId);
        $commentModal[0].showModal();
    });

    // Load comments for a blog
    function loadComments(blogId) {
        $.ajax({
            url: '/Home/GetComments',
            method: 'GET',
            data: { BlogId: blogId },
            success: function (comments) {
                if (comments.length === 0) {
                    $commentsList.html('<i>No comments yet.</i>');
                    return;
                }
                comments.forEach(c => {
                    console.log(comments);
                    $commentsList.append(`<p><strong>${escapeHtml(c.name)}:</strong> ${escapeHtml(c.comment)}</p>`);
                });
            },
            error: function () {
                $commentsList.html('<b>Error loading comments</b>');
            }
        });
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
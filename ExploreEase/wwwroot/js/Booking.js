$('#locationForm').on('submit', function (e) {
    e.preventDefault(); // (optional) prevent normal form submit
    $.ajax({
        url: 'UserActivity/User/ShowLocation',
        method: 'POST',
        data: $(this).serialize(),
        success: function (response) {
            console.log(response);
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
        }
    });
});

$('#bookmarkBtn').on('click', function () {
    const $btn = $(this);
    const currentlyBookmarked = $btn.data('bookmarked');
    const tourPackageId = $btn.data('packageid');

    const url = currentlyBookmarked
        ? '/UserActivity/User/RemoveBookmark'
        : '/UserActivity/User/AddBookmark';

    $.post(url, { tourPackageId: tourPackageId })
        .done(function (res) {
            $btn.data('bookmarked', !currentlyBookmarked);
            $btn.find('i').toggleClass('bi-bookmark bi-bookmark-fill text-primary');

            const toastEl = document.getElementById('bookmarkToast');
            const toastBody = document.getElementById('bookmarkToastBody');
            toastBody.textContent = !currentlyBookmarked
                ? 'Added to bookmarks'
                : 'Removed from bookmarks';
            const toast = new bootstrap.Toast(toastEl);
            toast.show();
        })
        .fail(function () {
            const toastEl = document.getElementById('bookmarkToast');
            const toastBody = document.getElementById('bookmarkToastBody');
            toastBody.textContent = 'Failed to update bookmark';
            const toast = new bootstrap.Toast(toastEl);
            toast.show();
        });
});

// Reviews modal
window.loadReviews = function (tourPackageId) {
    const modal = new bootstrap.Modal(document.getElementById('reviewModal'));
    modal.show();
    $('#reviewModalBody').html('<p class="text-muted">Loading reviews...</p>');

    $.ajax({
        url: '/UserActivity/User/ShowReview',
        type: 'GET',
        data: { Tourpackageid: tourPackageId },
        success: function (data) {
            if (data && data.length > 0) {
                let html = '<ul class="list-group">';
                data.forEach(function (review) {
                    html += `<li class="list-group-item">
                                <strong>${review.name}</strong><br/>
                                <span>${'★'.repeat(review.rating)}${'☆'.repeat(5 - review.rating)}</span><br/>
                                ${review.comment}
                             </li>`;
                });
                html += '</ul>';
                $('#reviewModalBody').html(html);
            } else {
                $('#reviewModalBody').html('<p class="text-muted">No reviews found.</p>');
            }
        },
        error: function () {
            $('#reviewModalBody').html('<p class="text-danger">Failed to load reviews.</p>');
        }
    });
};

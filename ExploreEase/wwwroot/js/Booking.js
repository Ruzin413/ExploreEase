$('#locationForm').on('submit', function (e) {
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
function loadReviews(tourPackageId) {
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
                    console.log(review);
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
}

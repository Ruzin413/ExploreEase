// ---------- Tab Switching ----------
function showTab(tabId) {
    document.querySelectorAll('.tab-content').forEach(content => content.classList.remove('active'));
    document.querySelectorAll('.tab-button').forEach(btn => {
        btn.classList.remove('active');
        btn.setAttribute('aria-selected', 'false');
        btn.tabIndex = -1;
    });
    const selectedContent = document.getElementById(tabId);
    if (selectedContent) selectedContent.classList.add('active');
    const selectedBtn = document.getElementById(`tab-${tabId}`);
    if (selectedBtn) {
        selectedBtn.classList.add('active');
        selectedBtn.setAttribute('aria-selected', 'true');
        selectedBtn.tabIndex = 0;
    }
}

// ---------- Keyboard navigation ----------
document.addEventListener("DOMContentLoaded", function () {
    const tabButtons = document.querySelectorAll('.tab-button');
    tabButtons.forEach((btn, index, arr) => {
        btn.addEventListener('keydown', e => {
            if (['ArrowRight', 'ArrowDown'].includes(e.key)) { e.preventDefault(); arr[(index + 1) % arr.length].focus(); }
            if (['ArrowLeft', 'ArrowUp'].includes(e.key)) { e.preventDefault(); arr[(index - 1 + arr.length) % arr.length].focus(); }
            if (['Enter', ' '].includes(e.key)) { e.preventDefault(); btn.click(); }
        });
    });
});

// ---------- Extend Modal ----------
window.openExtendModal = function (button) {
    const paymentId = $(button).data('paymentid');
    const tourPackageId = $(button).data('tourpackageid');
    $('#extend-paymentid').val(paymentId);
    $('#extend-tourpackageid').val(tourPackageId);
    $('#extendMessage').text('').hide();
    $('#extendModal').fadeIn();
    $('#confirmExtend').show(); // show the button again if hidden previously
};

window.closeExtendModal = function () {
    $('#extendModal').fadeOut();
};

$('#confirmExtend').click(function () {
    const paymentId = $('#extend-paymentid').val();
    const tourPackageId = $('#extend-tourpackageid').val();

    $.ajax({
        url: '/UserActivity/User/ExtendDate',
        method: 'POST',
        data: { id: paymentId, TourPackageId: tourPackageId },
        success: function (res) {
            if (res === true) {
                $('#extendMessage').text('Tour date extended successfully!').css('color', 'green').show();
                $('#confirmExtend').hide(); // hide button after success
            } else {
                $('#extendMessage').text('Failed to extend the tour date.').css('color', 'red').show();
            }
        },
        error: function () {
            $('#extendMessage').text('Error while extending the tour date.').css('color', 'red').show();
        }
    });
});

// ---------- Review Modal ----------
window.openReviewModal = function (button) {
    const paymentId = $(button).data('paymentid');
    const packageId = $(button).data('packageid');
    $('#review-paymentid').val(paymentId);
    $('#review-packageid').val(packageId);
    $('#reviewMessage').text('').hide();
    $('#reviewModal').fadeIn();
    $('#submitReviewBtn, #review-text, .star-rating').show(); // show inputs again if hidden
};

window.closeReviewModal = function () {
    $('#reviewModal').fadeOut();
};

$('#submitReviewBtn').click(function () {
    const paymentId = $('#review-paymentid').val();
    const packageId = $('#review-packageid').val();
    const rating = $('.star-rating input:checked').val();
    const reviewText = $('#review-text').val();

    $.ajax({
        url: '/UserActivity/User/SubmitReview',
        method: 'POST',
        data: { id: paymentId, packageId: packageId, rating: rating, reviewText: reviewText },
        success: function (res) {
            if (res.success) {
                $('#reviewMessage').text('Review submitted successfully!').css('color', 'green').show();
                // hide inputs and buttons after success
                $('#submitReviewBtn, #review-text, .star-rating').hide();
            } else {
                $('#reviewMessage').text('Failed to submit review: ' + res.error).css('color', 'red').show();
            }
        },
        error: function () {
            $('#reviewMessage').text('Error while submitting review.').css('color', 'red').show();
        }
    });
});

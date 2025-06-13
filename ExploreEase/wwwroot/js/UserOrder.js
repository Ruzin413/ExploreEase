let selectedOrderId = null;

function openModal(orderId) {
    selectedOrderId = orderId;
    document.getElementById("confirmModal").style.display = "block";
}

function closeModal() {
    document.getElementById("confirmModal").style.display = "none";
    selectedOrderId = null;
}

function confirmExtension() {
    if (!selectedOrderId) return;

    $.ajax({
        url: 'UserActivity/User/ExtendDate',
        type: 'POST',
        data: { TourPackageId: selectedOrderId },
        success: function (data) {
            alert(data.message);
            if (data.success) {
                location.reload();
            } else {
                closeModal();
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
            alert("Extension failed.");
            closeModal();
        }
    });
}

// Optional: Close modal when clicking outside
$(window).on('click', function (event) {
    const modal = document.getElementById("confirmModal");
    if (event.target === modal) {
        closeModal();
    }
});

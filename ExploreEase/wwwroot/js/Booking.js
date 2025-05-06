$('#locationForm').on('submit', function (e) {
    $.ajax({
        url: 'UserActivity/User/ShowLocation', // Replace with your controller name
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
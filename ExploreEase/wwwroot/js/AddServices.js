$(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        SubmitData();
    });
});
function SubmitData() {
    var formData = new FormData($('form')[0]);

    $.ajax({
        url: '/AdminController/AddServices',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            alert('Data submitted successfully!');
            console.log(response);
        },
        error: function (xhr, status, error) {
            alert('Error: ' + error);
            console.log(xhr.responseText);
        }
    });
}

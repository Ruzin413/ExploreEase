 $(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        SubmitData();
    });
});
function SubmitData() {
    var formData = new FormData($('form')[0]);
    $.ajax({
        url: 'Admin/AddServices',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        beforeSend: function () {
            console.log('Submitting data...');
        },
        success: function (response) {
            if (response.success) {
                alert(response.message || 'Data submitted successfully!');
                console.log(response);
                // Optional: clear the form or reset
                $('form')[0].reset();
            } else {
                alert('Failed: ' + (response.message || 'Something went wrong.'));
            }
        },
        error: function (xhr, status, error) {
            alert('Error: ' + error);
            console.log(xhr.responseText);
        }
    });
}

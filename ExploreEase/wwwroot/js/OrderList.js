$(document).ready(function () {
    $.ajax({
        url: "/Admin/Admin/OrderList1",
        method: 'GET',
        success: function (data) {
            const tbody = $("#orderData");
            tbody.empty();

            if (data.length === 0) {
                tbody.html('<tr><td colspan="9" class="empty-state">No orders found</td></tr>');
                return;
            }

            data.forEach(order => {
                const row = `<tr id="row-${order.id}">
                            <td>${order.username}</td>
                            <td>${order.price}</td>
                            <td>${order.startDate}</td>
                            <td>${order.endDate}</td>
                            <td>${order.bookingDate}</td>
                            <td>${order.numberOfPeople}</td>
                            <td>${order.totalPrice}</td>
                            <td>
                                <button class="btn btn-delete delete-btn" data-id="${order.id}">
                                    <span class="btn-text">Delete</span>
                                    <span class="loader hidden"></span>
                                </button>
                            </td>
                            <td>
                                <button class="btn btn-detail detail-btn" data-id="${order.id}">
                                    <span class="btn-text">Detail</span>
                                    <span class="loader hidden"></span>
                                </button>
                            </td>
                        </tr>`;
                tbody.append(row);
            });
        },
        error: function () {
            alert("Failed to load orders.");
            $("#orderData").html('<tr><td colspan="9" class="empty-state">Error loading data</td></tr>');
        }
    });
});

$(document).on('click', '.delete-btn', function () {
    const btn = $(this);
    const id = btn.data('id');
    const loader = btn.find(".loader");
    const btnText = btn.find(".btn-text");

    btnText.addClass("hidden");
    loader.removeClass("hidden");

    setTimeout(() => {
        $.ajax({
            url: `/Admin/Admin/DeleteOrder/${id}`,
            method: "DELETE",
            success: function () {
                $(`#row-${id}`).fadeOut(300, function () {
                    $(this).remove();

                    // Check if table is now empty
                    if ($("#orderData tr").length === 0) {
                        $("#orderData").html('<tr><td colspan="9" class="empty-state">No orders found</td></tr>');
                    }
                });
            },
            error: function () {
                alert("Failed to delete the record.");
                loader.addClass("hidden");
                btnText.removeClass("hidden");
            }
        });
    }, 1000);
});

$(document).on('click', '.detail-btn', function () {
    const id = $(this).data('id');
    window.location.href = `/Admin/Admin/OrderDetail?id=${id}`;
});
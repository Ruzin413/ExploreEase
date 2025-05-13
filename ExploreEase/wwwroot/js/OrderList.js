$(document).ready(function () {
    $.ajax({
        url: "/Admin/Admin/OrderList1",
        method: 'GET',
        success: function (data) {
            const tbody = $("#orderData");
            tbody.empty();
            data.forEach(order => {
                const row = `
                    <tr id="row-${order.id}">
                        <td class="border px-4 py-2">${order.username}</td>
                        <td class="border px-4 py-2">${order.price}</td>
                        <td class="border px-4 py-2">${order.startDate}</td>
                        <td class="border px-4 py-2">${order.endDate}</td>
                        <td class="border px-4 py-2">${order.bookingDate}</td>
                        <td class="border px-4 py-2">${order.numberOfPeople}</td>
                        <td class="border px-4 py-2">${order.totalPrice}</td>
                        <td>
                            <button class="delete-btn bg-red-500 text-white px-2 py-1 rounded hover:bg-red-600 flex items-center gap-2"
                                    data-id="${order.id}">
                                <span class="btn-text">Delete</span>
                                <span class="loader hidden border-2 border-t-transparent border-white w-4 h-4 rounded-full animate-spin"></span>
                            </button>
                        </td>
                         <td>
 <button id ="detail-btn" class="detail-btn bg-green-500 text-white px-2 py-1 rounded hover:bg-red-600 flex items-center gap-2"
        data-id="${order.id}">
    <span class="btn-text">Detail</span>
    <span class="loader hidden border-2 border-t-transparent border-white w-4 h-4 rounded-full animate-spin"></span>
</button>
                        </td>
                    </tr>`;
                tbody.append(row);
            });
        },
        error: function () {
            alert("Failed to load orders.");
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
                $(`#row-${id}`).remove();
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
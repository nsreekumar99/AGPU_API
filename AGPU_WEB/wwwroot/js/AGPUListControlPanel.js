$(document).ready(function () {
    console.log("Document is ready");

    // Extract the anti-forgery token from the form
    var csrfToken = $('#deleteAGPUForm input[name="__RequestVerificationToken"]').val();

    // Set up AJAX to include the anti-forgery token
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': csrfToken
        }
    });

    var table = $('#AGPUListControlPanel').DataTable({
        "ajax": {
            "url": "/api/AGPUControlPanelDataTableAPI/TransferData",
            "dataSrc": ""
        },
        "columns": [
            { "data": "name" },
            { "data": "brand" },
            { "data": "model" },
            { "data": "averageBenchPercentage" },
            { "data": "valuePercentage" },
            {
                "data": "price",
                "render": function (data) {
                    return new Intl.NumberFormat('hi-IN', { style: 'currency', currency: 'INR' }).format(data);
                }
            },
            {
                "data": null,
                "defaultContent": `
                    <form class="edit-form" method="get" action="/AGPU/Edit" style="display:inline;">
                        <input type="hidden" name="id" value="">
                        <button type="submit" class="btn btn-success edit-btn"><i class="bi bi-pencil-square"></i></button>
                    </form>
                    <form class="delete-form" method="post" action="/AGPU/Delete" style="display:inline;">
                        <input type="hidden" name="id" value="">
                        <input type="hidden" name="__RequestVerificationToken" value="${csrfToken}">
                        <button type="submit" class="btn btn-danger delete-btn"><i class="bi bi-trash-fill"></i></button>
                    </form>`
            }
        ],
        "order": [[0, "asc"]],
        "initComplete": function () {
            console.log("DataTable initialized");
        }
    });

    // Fill the hidden input with the AGPU ID and submit the form
    $('#AGPUListControlPanel tbody').on('click', 'button.edit-btn', function (e) {
        e.preventDefault(); // Prevent default button behavior

        var data = table.row($(this).parents('tr')).data();
        console.log("Edit button clicked, row data:", data);

        var form = $(this).closest('form'); // Find the closest form
        form.find('input[name="id"]').val(data.id); // Set the ID in the hidden input
        form.submit(); // Submit the form
    });

    //$('#AGPUListControlPanel tbody').on('click', 'button.delete-btn', function () {
    //    var data = table.row($(this).parents('tr')).data();
    //    $(this).siblings('input[name="id"]').val(data.id);
    //});


    // Handle Delete button click
    $('#AGPUListControlPanel tbody').on('click', 'button.delete-btn', function (e) {
        e.preventDefault(); // Prevent default button behavior

        // Retrieve data from the row
        var data = table.row($(this).closest('tr')).data();
        var agpuId = data.id;
        console.log("Delete button clicked, ID:", agpuId);

        // SweetAlert2 confirmation dialog
        Swal.fire({
            title: 'Are you sure?',
            text: 'You will not be able to recover this item!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                console.log("User confirmed deletion, sending AJAX request...");

                // User confirmed deletion, send AJAX request to GET method
                $.ajax({
                    url: '/AGPU/Delete', // MVC route for redirection
                    type: 'POST',
                    data: {
                        id: agpuId,
                        __RequestVerificationToken: csrfToken
                    },
                    success: function (result) {
                        console.log("Delete request successful. Reloading table data...");
                        // Reload table data after successful deletion
                        table.ajax.reload();
                        // Show success message
                        Swal.fire(
                            'Deleted!',
                            'The item has been deleted.',
                            'success'
                        );
                    },
                    error: function (xhr, status, error) {
                        console.error("Delete request failed:", status, error);
                        // Show error message if deletion fails
                        Swal.fire(
                            'Error!',
                            'Failed to delete the item.',
                            'error'
                        );
                    }
                });
            } else {
                console.log("User canceled the deletion.");
            }
        }).catch(function (error) {
            console.error("SweetAlert2 error:", error);
        });
    });


    console.log("Event listeners added");
});

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#activityModal').on('show.bs.modal', function (event) {
    var a = $(event.relatedTarget) // Button that triggered the modal
    var url = a.data('url');

    $.get(url, function (data) {
        $('#activityModal').html(data);

        $('#activityModal').modal('show');
    });


})
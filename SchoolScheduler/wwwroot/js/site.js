// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#exampleModal').on('show.bs.modal', function (event) {
    var a = $(event.relatedTarget) // Button that triggered the modal

    var room = a.data('room') // Extract info from data-* attributes
    var group = a.data('group')
    var klass = a.data('class')
    var teacher = a.data('teacher')

    var modal = $(this)
    modal.find('.modal-title').text('something different')
    modal.find('input[id=room]').val(room)
    modal.find('input[id=group]').val(group)
    modal.find('input[id=class]').val(klass)
    modal.find('input[id=teacher]').val(teacher)
})
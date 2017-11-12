$(document).ready(function () {
})
    
$('input.datepicker').daterangepicker({
    singleDatePicker: true,
    showDropdowns: true
}); 
    
$('form').on('submit', function (ev) {
    ev.preventDefault();

    let $form = $(this);

    if ($form.valid()) {
        alert('valid');
    }
    else {
        alert('not!');
    }
})

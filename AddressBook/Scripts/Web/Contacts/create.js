$(document).ready(function () {
})
    
$('input.datepicker').datepicker({
    uiLibrary: 'bootstrap4',
    iconsLibrary: 'fontawesome',
    format: 'yyyy-m-d'
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
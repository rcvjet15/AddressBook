$(document).ready(function () {
    alert('hello')
})

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
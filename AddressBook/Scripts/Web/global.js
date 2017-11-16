$(document).ready(function () {
    // Toggle between fixed and static navbar
    $('#toggleNavPosition').click(function () {
        $('body').toggleClass('fixed-nav');
        $('nav').toggleClass('fixed-top static-top');
    });

    // Toggle between dark and light navbar        
    $('#toggleNavColor').click(function () {
        $('nav').toggleClass('navbar-dark navbar-light');
        $('nav').toggleClass('bg-dark bg-light');
        $('body').toggleClass('bg-dark bg-light');
    });

    // Initialize all tooltips on page
    $('[data-toggle="tooltip"]').tooltip({
        container: 'body',
    })

    // Initialize all tooltips on page
    $('[data-toggle="popover"]').popover()
});
// This input file handler is used for profile picture uploads on any view that implements same classes
$('input.profile-image-browser').on('change', function (ev) {    
    readURL(this);
});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();
        
        reader.onload = function (e) {
            $(input).closest('div.card-block') // Get closest parent div
                .prev('img.profile-image-upload') // Get previous image element with .profile-image-upload
                .attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);

        // Show image name
        $(input).closest('div.card-block')
            .find('div.profile-image-info')
            .find('span.label')
            .text(input.files[0].name);
    }
}


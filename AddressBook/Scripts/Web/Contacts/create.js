
$('input.datepicker').daterangepicker({
    singleDatePicker: true,
    showDropdowns: true,
    locale: {
        format: "DD/MM/YYYY",
    }
}); 
    
$('form[id="create-contact"]').on('submit', function (ev) {
    ev.preventDefault();

    let $form = $(this);
    
    if ($form.valid()) {
        submitFormAjax($form)
            .then((data) => {
                alert('Success')
            }).fail((data) => {
            })
    }
})

$('#add-group').on('click', function () {

    let groupName = $('input[id="new-group-name"]').val();

    if (groupName.length === 0) {
        return;
    }
    else if (groupName.length > 20) {
        toggleGroupNameErrorMessage('Group name cannot have more than 20 characters.', true);
        return;
    }
    else if (!isGroupNameUnique(groupName)) {        
        toggleGroupNameErrorMessage('Group with name ' + groupName + ' already exists.', true);
        return;
    }
    // If validation passed successfully, hide validation message.
    toggleGroupNameErrorMessage(null, false);

    addNewGroupItem(groupName);
})

function submitFormAjax($form) {
    return $.ajax({
        url: $form.attr('action'),
        method: 'POST',
        // By default, data passed in to the data option as an object (technically, anything other than a string) will be processed and transformed into a query string, 
        // fitting to the default content-type "application/x-www-form-urlencoded". To send a DOMDocument, or other non-processed data, this option must be set to false.
        contentType: false,
        processData: false,
        data: new FormData($form[0])
    });   
}

// Function that inserts list item with new group name at 0 index in unordered list
// that displays all group names
function addNewGroupItem(groupName) {

    let listItem = '<li class="list-group-item">' +
        ' <label class="form-check-label" style="cursor:pointer">' + 
        '<input class="form-check-input" type="checkbox" name="Groups" value="' + groupName + '">' + 
        groupName + 
        '</label>' +
        '<span class="fa fa-remove pull-right" onclick="$(this).closest(\'li.list-group-item\').remove()" style="cursor:pointer;color:darkred"></span>'
    '</li>';

    // Insert at 0 index in unordered list
    $('ul[id="group-list"] li:eq(0)').before(listItem);
}

function toggleGroupNameErrorMessage(message, display) {

    if (!display) {
        $('span[id="group-validation-info"]').hide();
    }
    else {
        $('span[id="group-validation-info"]').text(message);
        $('span[id="group-validation-info"]').show()
    }
}

function isGroupNameUnique(name) {

    let unique = true;

    // Loop through all items and get group name for each item.        
    $('ul[id="group-list"]').find('li.list-group-item').each((index, listItem) => {

        let nameInList = $(listItem).find('input').val();
        
        if (name === nameInList) {
            unique = false;
            return;
        }
    });

    return unique;
}
const toastrOptions = {
    progressBar: true,
    closeButton: true,
}

// Indicates if form was changed. It will be used to prevent user from discarding changes.
let formChanged = false;

$(document).ready(function () {
    
});

$('input.datepicker').daterangepicker({
    singleDatePicker: true,
    showDropdowns: true,
    locale: {
        format: "DD/MM/YYYY",
    }
}); 

$('form').on('keyup', 'input', function (ev) {
    formChanged = true;
})

$('#cancel-btn').on('click', function (ev) {
    closeForm($('form[id="create-contact"]'));
})

$('form[id="create-contact"]').on('submit', function (ev) {
    ev.preventDefault();

    let $form = $(this);
    
    if ($form.valid()) {
        submitFormAjax($form)
            .then((data) => {
                if (data.Message) {
                    toastr.success(data.Message, 'Success', toastrOptions);
                }
                window.location.href = "/";
            }).fail((data) => {
                if (data.responseText) {
                    try {
                        // Server returns errors in array, this loops through each and displays it in toastr
                        JSON.parse(data.responseText)["Errors"].map((value, index) => {
                            toastr.error(value, 'Error', toastrOptions);
                        })
                    } catch (e) {
                        console.log('Error parsing json error result: ' + e);
                    }
                }
                else {
                    toastr.error("Error occured while saving contact.", 'Error', toastrOptions);
                }
            })
    }
})

$('#add-group').on('click', function (ev) {    
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
    
    // Show loading on button
    toggleButtonLoadingAnimation($(ev.currentTarget), show = true);

    setupGroupAjax('/Groups/Create', 'POST', { groupName: groupName })
        .then((data) => {
            if (data.Message) {
                toastr.success(data.Message, 'Success', toastrOptions);
            }
            addNewGroupItem(groupName, data.Id);
        }).fail((data) => {
            if (data.responseText) {
                try {
                    // Server returns errors in array, this loops through each and displays it in toastr
                    JSON.parse(data.responseText)["Errors"].map((value, index) => {
                        toastr.error(value, 'Error', toastrOptions);
                    })
                } catch (e) {
                    console.log('Error parsing json error result: ' + e);
                }
            }
        }).always(() => {
            toggleButtonLoadingAnimation($(ev.currentTarget), show = false);
        })    
})

// Handler that when 'x' button is clicked next to group name, that list item is removed.
// Second parameter 'span.remove-group-btn' tells to bind event on dynamically created child element (in this case 'span.remove-group-btn')
$('ul[id="group-list"]').on('click', 'span.remove-group-btn', function (ev) {
    let $parentListItem = $(this).closest('li.list-group-item');
    let groupID = $parentListItem.find('input[type="checkbox"]').val();

    console.log('selected group to remove ' + groupID);

    setupGroupAjax('/Groups/Delete', 'POST', { id: groupID })
        .then((data) => {
            if (data.Message) {
                toastr.success(data.Message, 'Success', toastrOptions);
            }
            $parentListItem.remove();
        }).fail((data) => {
            if (data.responseText) {
                try {
                    // Server returns errors in array, this loops through each and displays it in toastr
                    JSON.parse(data.responseText)["Errors"].map((value, index) => {
                        toastr.error(value, 'Error', toastrOptions);
                    })
                } catch (e) {
                    console.log('Error parsing json error result: ' + e);
                }
            }
        })  
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

// Function that makes request to server for creating or deleting group.
// 'url' parameter is calling URL, 'method' is http method (GET, POST) and 'jsonData' is data that is sent to server in json format.
// It returns ajax request so in caller function success or fail method must handle server response.
function setupGroupAjax(url, method, jsonData) {
    return $.ajax({
        url: url,
        method: method,
        data: JSON.stringify(jsonData),
        contentType: 'application/json',
        dataType: 'json'
    })
}


// Function that inserts list item with new group name at 0 index in unordered list
// that displays all group names.
// Takes group Id that will be group checkbox submit value.
function addNewGroupItem(groupName, value) {
    let listItem = '<li class="list-group-item">' +
        ' <label class="form-check-label" style="cursor:pointer">' + 
        '<input class="form-check-input" type="checkbox" name="AllGroups" value="' + value + '">' + 
        groupName + 
        '</label>' +
        '<span class="fa fa-remove pull-right remove-group-btn" style="cursor:pointer;color:darkred"></span>'
    '</li>';

    // If list is empty then just append new item
    if ($('ul[id="group-list"]').find('li').size() === 0) {
        // Insert at 0 index in unordered list
        $('ul[id="group-list"]').append(listItem);
    }
    else {
        // Insert at 0 index in unordered list
        $('ul[id="group-list"] li:eq(0)').before(listItem);
    }    
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

// Function that displays loading on button and disables it during loading.
// First parameter '$button' is jquery object selector for loading button, 
// second parameter 'show' is boolean that indicates if loading should show or stop
function toggleButtonLoadingAnimation($button, show) {    
    if (show) {
        $button.attr('disabled', '');
        $button.find('i').addClass('fa-spin');
    }
    else {
        $button.removeAttr('disabled');
        $button.find('i').removeClass('fa-spin');
    }
}

// Function that closes form. First checks if user made any changes.
function closeForm($form) {
    if (formChanged) {
        if (!confirm("Changes not saved. Do you want to continue?")) {
            return;
        }
    }

    $form.closest('#contact-view-panel').empty();
}
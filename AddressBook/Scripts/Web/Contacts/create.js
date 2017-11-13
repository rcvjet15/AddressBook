
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
                // Push state to index page and return to it so that contacts list refreshes
                window.history.pushState(null, 'index', '/')
                window.history.go();
            }).fail((data) => {
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

    let toastrOptions = {
        progressBar : true,
        closeButton : true,
    }

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
        '<input class="form-check-input" type="checkbox" name="Groups" value="' + value + '">' + 
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
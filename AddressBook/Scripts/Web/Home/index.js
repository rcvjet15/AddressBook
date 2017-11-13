// All displayed contacts on page
let allContacts = [];
const loaderHtml = '<div class="mx-auto loader"></div>';

$(document).ready(function () {
    getContactsAndRender();
});

// Handler for create contact button click event
$('#create-contact-btn').on('click', function (ev) {
    $('#contact-view-panel')
        .html(loaderHtml) // Display loading
        .load('/Contacts/Create');

    // Push state (Add to URL '/Create')
    window.history.pushState(null, "create", '/Create');
})

$('#search-contacts-btn').on('click', function (ev) {
    let searchText = $('#search-contacts').val().toLowerCase();    
    searchContacts(searchText);
})

$('#search-contacts').on("keyup", function (ev) {
    // Trigger clicked event of search button on keyup
    $('#search-contacts-btn').click();   
})

$('#search-types-list').on("change", function (ev) {
    // Trigger clicked event of search button on change of select list
    $('#search-contacts-btn').click(); 
})

function getContactsAndRender() {
    getContacts('/Contacts/GetUserContacts')
        .then((data) => {
            if (data.Contacts) {
                allContacts = data.Contacts;
                renderContactList(data.Contacts, templateId = '#contact-list-template', targetId = '#contact-list-target')
            }
        }).fail((data) => {

        });
}

function getContacts(url) {
    return $.ajax({
        url: url,
        method: 'GET',
        contentType: 'application/json',
    });    
}

function renderContactList(contacts, templateId, targetId) {
    let htmlList = [];
    contacts.map(c => {
        let template = $(templateId).html();
        //Mustache.parse(template);
        let rendered = Mustache.render(template, c);
        htmlList.push(rendered);
    });

    $(targetId).html(htmlList);
}

function searchContacts(searchText) {
    

    if (allContacts.length === 0) {
        return;
    }

    // Get selected text from search by select list
    let searchType = $('#search-types-list').find(':selected').text();

    // Filter contacts by jQuery method grep
    filteredContacts = $.grep(allContacts, (function (n, i) {
        // Based on selected item in select list, get index of wanted attribute        
        switch (searchType) {
            case "Last Name":
                return n.LastName.toLowerCase().indexOf(searchText) > -1;
            case "First Name":
                return n.FirstName.toLowerCase().indexOf(searchText) > -1;
            case "Groups":
                // Loop though all groups and return true if any of them contains search text.
                for (var i = 0; i < n.Groups.length; i++) {
                    if (n.Groups[i].toLowerCase().indexOf(searchText) > -1)
                        return true;
                }
                return false;
            default:
                return (n.LastName + " " + n.FirstName).toLowerCase().indexOf(searchText) > -1;
        }
    }));

    // Display loader while filtering
    $('#contact-list-target').html(loaderHtml);
    renderContactList(filteredContacts, templateId = '#contact-list-template', targetId = '#contact-list-target')
}
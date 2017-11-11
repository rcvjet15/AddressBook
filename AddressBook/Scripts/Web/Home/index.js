// Get
let allContacts = [];

$(document).ready(function () {
    getContactsAndRender();
});

$('#search-contacts').on("keyup", function (ev) {
    let searchText = $(this).val().toLowerCase();

    if (allContacts.length === 0) {
        return;
    }

    // Get selected text from search by select list
    let searchType = $('#search-types').find(':selected').text();

    // Filter contacts by jQuery method grep
    filteredContacts = $.grep(allContacts, (function (n, i) {
        // Based on selected item in select list, get value from wanted attribute
        let wantedAttributeValue = ((searchType) => {
            switch (searchType) {
                case "Last Name":
                    return n.LastName.toLowerCase();                    
                case "First Name":
                    return n.FirstName.toLowerCase();
                case "Groups":
                    break;
                default:
                    return (n.LastName + " " + n.FirstName).toLowerCase();
            }
        }).call(this, searchType);

        return wantedAttributeValue.indexOf(searchText) > -1;
    }));

    $('#contact-list-target').html('<div class="mx-auto loader"></div>');
    renderContactList(filteredContacts, templateId = '#contact-list-template', targetId = '#contact-list-target')
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
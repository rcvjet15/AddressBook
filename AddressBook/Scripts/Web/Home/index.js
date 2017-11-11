
$(document).ready(function () {
    let contacts = getContacts('/Contacts/GetUserContacts')
        .then((data) => {
            if (data.Contacts) {
                renderContactList(data.Contacts, templateId = '#contact-list-template', targetId = '#contact-list-target')
            }
        }).fail((data) => {
            
        });    
});

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
        Mustache.parse(template);
        let rendered = Mustache.render(template, c);
        htmlList.push(rendered);
    });

    $(targetId).html(htmlList);
}


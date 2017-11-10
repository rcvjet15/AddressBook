$(document).ready(function () {
    loadContacts('/GetUserContacts');
});

function loadContacts(url) {
    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'application/json',        
    }).done(function (data) {

        renderContactList(data.Contacts, '#contact-list-template', '#contact-list-target')

    }).fail(function (data) {
    })
}

function renderContactList(contacts, templateId, targetId) {
    let htmlList = [];
    contacts.map(c => {
        let template = $(templateId).html();
        Mustache.parse(template);
        let rendered = Mustache.render(template, c);
        htmlList.push(rendered);
    });

    $(targetId).html(htmlList)
}


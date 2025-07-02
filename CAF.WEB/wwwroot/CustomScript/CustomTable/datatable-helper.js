// Tüm sayfalarda CSRF token gönderimi
$.ajaxSetup({
    headers: {
        'RequestVerificationToken': $('meta[name="csrf-token"]').attr('content')
    }
});

// Genel DataTable başlatıcı
function initializeDataTable(tableSelector, ajaxUrl, columnDefs) {
    return $(tableSelector).DataTable({
        processing: true,
        serverSide: true,
        responsive: true,
        paging: true,
        searching: true,
        ordering: false,
        dom: '<"row mb-3"' +
            '<"col-md-6 text-start"f>' +     // Arama - sol üst
            '<"col-md-6 text-end"l>' +       // Sayfa uzunluk - sağ üst
            '>' +
            '<"row"<"col-12"tr>>' +
            '<"row mt-3"' +
            '<"col-md-4 text-start"i>' +     // Bilgi - sol alt
            '<"col-md-4 text-center summary-info">' + // Ortada bilgi alanı (opsiyonel)
            '<"col-md-4 text-end"p>' +       // Sayfalama - sağ alt
            '>',
        ajax: {
            url: ajaxUrl,
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                return JSON.stringify({
                    Start: d.start,
                    Length: d.length,
                    Search: d.search.value?.trim() || ""
                });
            }
        },
        columns: columnDefs,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.4/i18n/tr.json'
        }
    });
}

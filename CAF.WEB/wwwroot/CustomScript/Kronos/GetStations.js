$(document).ready(function () {
    initializeDataTable("#stationsTable", "/Kronos/Station/GetPagedStations", [
        {
            data: null,
            defaultContent: '',
            className: 'control',
            orderable: false
        },
        { data: 'stationName', title: 'İstasyon' },
        { data: 'stationCode', title: 'Kodu' },
        { data: 'serviceTypes', title: 'Servis' },
        { data: 'platform', title: 'Platform' },
        {
            data: 'lastSynced',
            title: 'Son Aktivite',
            render: function (data) {
                return data ? new Date(data).toLocaleString() : '';
            }
        },
        {
            data: 'id',
            title: 'İşlemler',
            orderable: false,
            render: function (id) {
                return `
                    <a href="/Kronos/Station/Edit/${id}" class="btn btn-sm btn-outline-primary me-1">Düzenle</a>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteStation(${id})">Sil</button>
                `;
            }
        }
    ]);

    // Sayfa bilgisi orta alana yazılsın istiyorsan:
    $('#stationsTable').on('draw.dt', function () {
        const api = $('#stationsTable').DataTable();
        const info = api.page.info();
        $('.summary-info').html(`Toplam ${info.recordsTotal} kayıt | Sayfa ${info.page + 1} / ${info.pages}`);
    });
});

function deleteStation(id) {
    if (!confirm("Bu istasyonu silmek istediğinizden emin misiniz?")) return;

    $.ajax({
        url: `/Kronos/Station/Delete/${id}`,
        type: 'POST',
        success: function () {
            $('#stationsTable').DataTable().ajax.reload();
        },
        error: function () {
            alert("Silme işlemi başarısız oldu.");
        }
    });
}

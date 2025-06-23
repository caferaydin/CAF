$(document).ready(function () {
    $('#DataTable').DataTable({
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "dom": '<"top"lf>rt<"bottom"ip><"clear">',
        "language": {
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": "Next",
                "previous": "Previous"
            },
            "lengthMenu": "Minumum _MENU_ Göster ",
            "info": "Toplam Kayıt _TOTAL_",
            "zeroRecords": "No records to display",
            "infoEmpty": "No records available",
            "search" : "Ara : ",
            "infoFiltered": "(filtered from _MAX_ total records)"
        }
    });
});
 
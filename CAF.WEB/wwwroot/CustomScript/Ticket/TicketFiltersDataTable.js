let lastClickedTime = {}; // Son tıklama zamanını saklamak için

function toggleFilter(headerCell) {
    const currentTime = new Date().getTime();
    const column = headerCell.getAttribute('data-column');
    const filterInput = headerCell.querySelector('.filter-input');

    if (lastClickedTime[column] && (currentTime - lastClickedTime[column] < 300)) {
        // Çift tıklama algılandı
        filterInput.classList.toggle('active');
        lastClickedTime[column] = null; // Son tıklama zamanını sıfırla
    } else {
        // Tek tıklama algılandı
        lastClickedTime[column] = currentTime; // Son tıklama zamanını güncelle
    }
}

function filterTable() {
    const filters = document.querySelectorAll('.filter-input.active');
    const filterValues = {};

    filters.forEach(filter => {
        const column = filter.getAttribute('data-column');
        filterValues[column] = filter.value.toLowerCase();
    });

    const rows = document.querySelectorAll('#ticketsTableBody tr');

    rows.forEach(row => {
        let shouldShow = true;

        for (const column in filterValues) {
            const cellText = row.querySelector(`td[data-column="${column}"]`).textContent.toLowerCase();
            if (cellText.indexOf(filterValues[column]) === -1) {
                shouldShow = false;
                break;
            }
        }

        row.style.display = shouldShow ? '' : 'none';
    });
}

// Event listener ekle
document.querySelectorAll('.filterable').forEach(header => {
    header.addEventListener('click', function () {
        toggleFilter(this);
    });
});
function loadUserRoles(userId) {
    if (userId) {
        $.when(
            $.get(`/GetRolesToUser/${userId}`), // Kullanıcının atanmış rolleri
            $.get('/GetAllRoles') // Tüm roller
        ).done(function (userRolesResponse, allRolesResponse) {
            // Yanıtları kontrol et
            console.log('Kullanıcı Rolleri:', userRolesResponse[0]);
            console.log('Tüm Roller:', allRolesResponse[0]);

            // Yanıtları doğru şekilde işleyin
            var userRoles = userRolesResponse[0]?.userRoles || [];
            var allRoles = allRolesResponse[0]?.roles || [];

            var rolesContainer = $('#rolesContainer');
            rolesContainer.empty(); // Önceki rolleri temizle

            if (Array.isArray(allRoles)) {
                allRoles.forEach(function (role) {
                    var checked = userRoles.includes(role.name) ? 'checked' : '';
                    rolesContainer.append(`
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Roles" value="${role.name}" id="${role.Id}" ${checked}>
                                    <label class="form-check-label" for="${role.Id}">
                                        ${role.name}
                                    </label>
                                </div>
                            `);
                });
            } else {
                console.error('Tüm roller verisi geçersiz:', allRoles);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error('AJAX isteği başarısız:', textStatus, errorThrown);
        });
    } else {
        $('#rolesContainer').empty(); // Kullanıcı seçilmediğinde roller konteynerini temizle
    }
}
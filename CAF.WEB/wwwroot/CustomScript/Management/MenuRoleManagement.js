//function createMenuItem(menu, roleMenus) {
//    // Menü ID'sinin roleMenus içinde olup olmadığını kontrol et
//    var checked = roleMenus.some(function (roleMenu) {
//        return roleMenu.id === menu.Id;
//    }) ? 'checked' : '';

//    // Menü HTML'i oluştur
//    return `
//        <div class="form-check">
//            <input class="form-check-input" type="checkbox" name="MenuIds" value="${menu.Id}" id="${menu.Id}" ${checked}>
//            <label class="form-check-label" for="${menu.Id}">
//                ${menu.Name}
//            </label>
//        </div>
//    `;
//}

//function loadRoleMenuList(roleId) {
//    if (roleId) {
//        $.when(
//            $.get(`/GetMenusForRole/${roleId}`), // Belirli bir rol için menü izinlerini al
//            $.get('GetAllMenus') // Tüm menüleri al
//        ).done(function (roleMenusResponse, allMenusResponse) {
//            var roleMenus = roleMenusResponse[0]?.roleMenus || [];
//            var allMenus = allMenusResponse[0]?.menus || [];

//            var menuContainer = $('#menuContainer');
//            menuContainer.empty(); // Önceki menüleri temizle

//            if (Array.isArray(allMenus)) {
//                // Tüm menüleri işaretleyip DOM'a ekle
//                allMenus.forEach(function (menu) {
//                    menuContainer.append(createMenuItem(menu, roleMenus));
//                });
//            } else {
//                console.error('Tüm menüler verisi geçersiz:', allMenus);
//            }
//        }).fail(function (jqXHR, textStatus, errorThrown) {
//            console.error('AJAX isteği başarısız:', textStatus, errorThrown);
//        });
//    } else {
//        $('#menuContainer').empty(); // Rol seçilmediğinde menüleri temizle
//    }
//}



function loadRoleMenuList(roleId) {
    if (roleId) {
        $.when(
            $.get(`/GetMenusForRole/${roleId}`), // Belirli bir rol için menü izinlerini al
            $.get('GetAllMenus') // Tüm menüleri al
        ).done(function (roleMenusResponse, allMenusResponse) {
            // Yanıtları kontrol et
            console.log('Role Menus:', roleMenusResponse[0]);
            console.log('All Menus:', allMenusResponse[0]);

            // Yanıtları doğru şekilde işleyin
            var roleMenus = roleMenusResponse[0]?.roleMenus || [];
            var allMenus = allMenusResponse[0]?.menus || [];

            var menuContainer = $('#menuContainer');
            menuContainer.empty(); // Önceki menüleri temizle

            if (Array.isArray(allMenus)) {
                allMenus.forEach(function (menu) {
                    var checked = roleMenus.some(function (roleMenu) {
                        return roleMenu.id === menu.Id;
                    }) ? 'checked' : '';

                    menuContainer.append(`
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" name="MenuIds" value="${menu.Id}" id="${menu.Id}" ${checked}>
                            <label class="form-check-label" for="${menu.Id}">
                                ${menu.Name}
                            </label>
                        </div>
                    `);
                });
            } else {
                console.error('Tüm menüler verisi geçersiz:', allMenus);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error('AJAX isteği başarısız:', textStatus, errorThrown);
        });
    } else {
        $('#menuContainer').empty(); // Rol seçilmediğinde menüleri temizle
    }
}
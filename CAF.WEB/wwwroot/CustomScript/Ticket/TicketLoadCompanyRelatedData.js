function loadCompanyRelatedData(companyId) {
    if (companyId) {
        $.get('/Ticket/GetCompanyEmployees', { companyId: companyId }, function (data) {
            var employeeSelect = $('#CompanyEmployeeId');
            var employeeEmail = $('#CompanyEmployeeEmail');
            var alertContainer = $('#alertContainer');
            employeeEmail.val('');
            employeeSelect.empty();
            employeeSelect.append('<option value="">Seçiniz</option>');

            if (data.length > 0) {
                data.forEach(function (employee) {
                    employeeSelect.append('<option value="' + employee.id + '">' + employee.fullName + '</option>');
                });
                alertContainer.empty();
            } else {
                alertContainer.html('<div class="alert alert-warning">Bu şirketin çalışanı bulunmamaktadır. <a target="_blank" href="/Company/Profil/' + companyId + '">Şirket profiline dön</a></div>');
            }



            loadProducts();
        });


        $.get('/Ticket/GetTransactionTypes', function (data) {
            var transactionTypeSelect = $('#TransactionTypeId');
            transactionTypeSelect.empty();
            transactionTypeSelect.append('<option value="">Seçiniz</option>');
            data.forEach(function (transactionType) {
                transactionTypeSelect.append('<option value="' + transactionType.id + '">' + transactionType.name + '</option>');
            });

            //// Sadece TransactionTypes yüklendikten sonra VatRateTypes'ı yükle
            //var selectedTransactionType = $('#TransactionTypeId option:selected').text();
            //if (selectedTransactionType === "Ücretli") {
            //    loadVatRateTypes();
            //}
        });




    } else {
        $('#CompanyEmployeeId').empty().append('<option value="">Seçiniz</option>');
        $('#ProductId').empty().append('<option value="">Seçiniz</option>');
        $('#contractPanel').hide();
    }
}

//function loadVatRateTypes() {
//    $.get('/Definition/VatRateTypeJson', function (vatRateTypes) {
//        var vatRateTypeSelect = $('#VatRateTypeId');
//        vatRateTypeSelect.empty();
//        vatRateTypeSelect.append('<option value="">Seçiniz</option>');
//        vatRateTypes.forEach(function (vatRateType) {
//            vatRateTypeSelect.append('<option value="' + vatRateType.id + '">' + vatRateType.name + '</option>');
//        });
//    });
//}

function loadContracts(transactionTypeId, companyId) {

    if (transactionTypeId == 1) {
        $.get('/Ticket/GetContracts', { companyId: companyId }, function (data) {
            var contractsList = $('#contractsList');
            contractsList.empty();

            if (data.length > 0) {
                $('#contractPanel').show();

                var row = '<tr>' +
                    '<td><input type="checkbox" id="ContractCompanyId" name="ContractCompanyId" value=""></td>' +
                    '<td id="contractDefinition"></td>' +
                    '<td id="contractEndDate"></td>' +
                  /*  '<td id="actions"></td>' +*/
                    '</tr>';

                contractsList.append(row);

                if (data.length > 0) {
                    var contract = data[0];
                    $('#ContractCompanyId').val(contract.contractCompanyId).prop('checked', false);
                    $('#contractDefinition').text(contract.contractDefinition);
                    $('#contractEndDate').text(contract.endDate ? new Date(contract.endDate).toLocaleDateString() : '');
                  /*  $('#actions').html('<a target="_blank" href="/Contract/Details?contractId=' + contract.contractId + '" class="btn btn-secondary btn-sm view-details"><span class="fe fe-eye"></span></a>')*/

                }

                $('#ContractCompanyId').change(function () {
                    loadProducts(companyId, $(this).is(':checked') ? contract.contractId : '');
                    toggleTransactionTypeDropdown($(this).is(':checked'));
                });

                toggleTransactionTypeDropdown($('#ContractCompanyId').is(':checked'));
            } else {
                alert("Şirkete Ait Sözleşme Bulunmamaktadır."); // Not
                $('#contractPanel').hide(); // Paneli gizle
                loadProducts(companyId, ''); // Tüm ürünleri yükle
                toggleTransactionTypeDropdown(false); // Sözleşme seçilmediği için dropdown'u devre dışı bırak

            }
        });
        showPriceVatInput(false);
    } else if (transactionTypeId == 2) {
        $('#contractPanel').hide();
        showPriceVatInput(true); // Ücretli işlem türü seçildiğinde inputları göster
    } else {
        showPriceVatInput(false); // Diğer işlem türlerinde inputları gizle
    }
}

function showPriceVatInput(show) {
    if (show) {
        $.get('/Definition/VatRateTypeJson', function (vatRateTypes) {
            var vatRateTypeSelect = $('#VatRateTypeId');
            vatRateTypeSelect.empty();
            vatRateTypeSelect.append('<option value="">Seçiniz</option>');
            vatRateTypes.forEach(function (vatRateType) {
                vatRateTypeSelect.append('<option value="' + vatRateType.id + '">' + vatRateType.name + '-> %' + vatRateType.rate + '</option>');
            });
        });
        $('#priceVatInputs').show();
        $("#vatAmountContainerId").show();
    } else {
        $('#priceVatInputs').hide();
        $('#vatAmountContainerId').hide();

        $('#vatAmount').text('');
        $('#totalAmount').text('');
    }
}

function toggleTransactionTypeDropdown(enable) {
    var transactionTypeSelect = $('#TransactionTypeId');
    if (enable) {
        transactionTypeSelect.prop('disabled', false);
    } else {
        transactionTypeSelect.prop('disabled', false);
    }
}


function loadProducts(companyId, contractId) {
    $.get('/Ticket/GetProducts', { companyId: companyId, contractId: contractId }, function (data) {
        var productSelect = $('#ProductId');
        productSelect.empty();
        productSelect.append('<option value="">Seçiniz</option>');
        data.forEach(function (product) {
            productSelect.append('<option value="' + product.id + '">' + product.name + '</option>');
        });
    });

}

function loadEmployeeEmail(employeeId) {
    if (employeeId) {
        $.get('/Ticket/GetEmployeeEmail', { employeeId: employeeId }, function (data) {
            $('#CompanyEmployeeEmail').val(data.email);
        });
    } else {
        $('#CompanyEmployeeEmail').val('');
    }
}


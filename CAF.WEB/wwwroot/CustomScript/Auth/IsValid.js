
$(document).ready(function () {
    // Kullanıcı Adı Kontrolü
    $('#username').on('keyup', function () {
        var username = $(this).val();

        if (username.length >= 3) {
            $.ajax({
                url: '/Users/CheckUsername',
                type: 'GET',
                data: { username: username },
                success: function (isUnique) {
                    if (isUnique) {
                        $('#username-feedback').text('Kullanıcı adı kullanılabilir.').removeClass('text-danger').addClass('text-success');
                    } else {
                        $('#username-feedback').text('Kullanıcı adı zaten kullanımda.').removeClass('text-success').addClass('text-danger');
                    }
                },
                error: function () {
                    $('#username-feedback').text('Bir hata oluştu.').removeClass('text-success').addClass('text-danger');
                }
            });
        } else {
            $('#username-feedback').text('');
        }
    });

    // E-posta Kontrolü
    $('#email').on('keyup', function () {
        var email = $(this).val();
        var emailFeedback = $('#email-feedback');

        // E-posta formatını kontrol et
        if (isValidEmail(email)) {
            // E-posta uzunluğu yeterli ise benzersizliğini kontrol et
            if (email.length >= 10) {
                $.ajax({
                    url: '/Users/CheckEmail',
                    type: 'GET',
                    data: { email: email },
                    success: function (isUnique) {
                        if (isUnique) {
                            emailFeedback.text('E-posta adresi kullanılabilir.').removeClass('text-danger').addClass('text-success');
                        } else {
                            emailFeedback.text('E-posta adresi zaten kullanımda.').removeClass('text-success').addClass('text-danger');
                        }
                    },
                    error: function () {
                        emailFeedback.text('Bir hata oluştu.').removeClass('text-success').addClass('text-danger');
                    }
                });
            } else {
                emailFeedback.text('');
            }
        } else {
            emailFeedback.text('Geçersiz e-posta adresi.').removeClass('text-success').addClass('text-danger');
        }
    });



    // Telefon Numarası Kontrolü
    $('#phone').on('keyup', function () {
        var phone = $(this).val();
        var cleanedPhone = cleanPhoneNumber(phone);
        var phoneFeedback = $('#phone-feedback');

        if (cleanedPhone.length >= 10) {
            $.ajax({
                url: '/Users/CheckPhone',
                type: 'GET',
                data: { phone: cleanedPhone },
                success: function (isUnique) {
                    if (isUnique) {
                        phoneFeedback.text('Telefon numarası kullanılabilir.').removeClass('text-danger').addClass('text-success');
                    } else {
                        phoneFeedback.text('Telefon numarası zaten kullanımda.').removeClass('text-success').addClass('text-danger');
                    }
                },
                error: function () {
                    phoneFeedback.text('Bir hata oluştu.').removeClass('text-success').addClass('text-danger');
                }
            });
        } else {
            phoneFeedback.text('');
        }
    });


});

// E-posta formatı kontrol fonksiyonu
function isValidEmail(email) {
    // E-posta formatı kontrolü için basit bir düzenli ifade
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
}

function cleanPhoneNumber(phone) {
    // Parantez içindeki alanı çıkar
    var cleaned = phone.replace(/\([^\)]*\)/g, '').trim(); // Parantez içindeki kısmı çıkarır

    // Başındaki +, 90, 0 ve 1 karakterlerini kaldır
    cleaned = cleaned.replace(/^\+/, '') // Başındaki '+' karakterini kaldır
        .replace(/^90/, '') // Başındaki '90' karakterlerini kaldır
        .replace(/^0/, '')  // Başındaki '0' karakterini kaldır
        .replace(/^1/, '')  // Başındaki '1' karakterini kaldır
        .replace(/\D/g, ''); // Sadece rakamları bırak

    return cleaned;
}
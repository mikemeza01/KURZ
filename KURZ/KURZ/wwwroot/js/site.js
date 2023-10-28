(function ($) {
    $(function () {

        var languageDatatable = {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
        };

        $(document).ready(function () {
            new DataTable('#TableUsers', {
                language: languageDatatable
            });
        });
    });
})(jQuery);
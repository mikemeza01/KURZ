(function ($) {
    $(function () {

        var languageDatatable = {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
        };

        $(document).ready(function () {

            var tableUsers = $('#TableUsers');
            var tableCategories = $('#TableCategories');

            if (tableUsers.length > 0) {
                new DataTable('#TableUsers', {
                    language: languageDatatable
                });
            }
            
            if (tableCategories.length > 0) {
                new DataTable('#TableCategories', {
                    language: languageDatatable,
                    "autoWidth": false,
                    "columns": [
                        { "width": "33.3%" },
                        { "width": "33,3%" },
                        { "width": "33,3%" },
                    ]
                });
            }

            
        });
    });
})(jQuery);
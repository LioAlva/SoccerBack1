
    $(document).ready(function () {
        $("#LeagueId").change(function () {
            $("#TeamId").empty();
            $.ajax({
                type: 'POST',
                url: Url,
                dataType: 'json',
                data: { LeagueId: $("#LeagueId").val() },
                success: function (teams) {
                    $.each(teams, function (i, teams) {
                        $("#TeamId").append('<option value="'
                            + teams.TeamId + '">'
                            + teams.Name + '</option>');
                    });
                },
                error: function (ex) {
                    alert('Failed to retrieve municipalities.' + ex);
                }
            });
            return false;
        })
    });
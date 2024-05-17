namespace Guide.Common.Static;

public static class TinyMceConfig
{
    public static Dictionary<string, object> Config = new()
    {
        { "menubar", "file edit view insert format tools table help" },
        {
            "toolbar",
            "undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | align numlist bullist | link image | table | lineheight outdent indent| forecolor backcolor removeformat | charmap emoticons | code fullscreen | pagebreak anchor codesample"
        },
        {
            "plugins",
            "preview importcss searchreplace autolink directionality code visualblocks visualchars fullscreen image link codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons"
        },
        { "quickbars_selection_toolbar", "bold italic | quicklink h2 h3 blockquote quickimage quicktable" },
        { "toolbar_mode", "sliding" },
        { "contextmenu", "link image table" },
    };
}
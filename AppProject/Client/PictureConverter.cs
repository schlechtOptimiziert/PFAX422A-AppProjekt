using AppProject.Shared;

namespace AppProject.Client;

public static class PictureConverter
{
    public static string ItemPictureToUri(ItemPicture picture)
        => string.Format("data:image/png;base64,{0}", Convert.ToBase64String(picture.Bytes));
}

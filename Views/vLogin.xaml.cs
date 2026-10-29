namespace mguerronExamen.Views;

public partial class vLogin : ContentPage
{
    string[,] usuarios = new string[4, 2]
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim() ?? "";
        string contrasena = txtContrasena.Text ?? "";

        bool accesoCorrecto = false;

        for (int i = 0; i < usuarios.GetLength(0); i++)
        {
            if (usuarios[i, 0] == usuario &&
                usuarios[i, 1] == contrasena)
            {
                accesoCorrecto = true;
                break;
            }
        }

        if (accesoCorrecto)
        {
            await Navigation.PushAsync(new vRegistro(usuario));
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        }
    }

    private async void btnAcercaDe_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert(
        "Acerca de",
        "Nombre: Martin Guerron\n" +
        "Carrera: Sistemas de Informacion\n" +
        "Paralelo: 8VO",
        "Aceptar"
    );
    }
}
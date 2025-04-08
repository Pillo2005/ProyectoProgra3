Public Class FrmMenuPrincipal
    Private Sub FrmMenuPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lblBienvenida.Text = "Bienvenido, " & ModuloGlobal.NombreUsuario & " (" & ModuloGlobal.RolUsuario & ")"

        ' Mostrar/Ocultar módulos según el rol
        If ModuloGlobal.RolUsuario = "Vendedor" Then
            btnEmpleados.Enabled = False
            btnReportes.Enabled = True ' Si quieres que también puedan ver reportes
        End If

    End Sub

    Private Sub btnProductos_Click(sender As Object, e As EventArgs) Handles btnProductos.Click
        FrmProductos.Show()
    End Sub

    Private Sub btnVentas_Click(sender As Object, e As EventArgs) Handles btnVentas.Click
        'FrmVentas.Show()
    End Sub

    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        FrmClientes.Show()
    End Sub

    Private Sub btnEmpleados_Click(sender As Object, e As EventArgs) Handles btnEmpleados.Click
        'FrmEmpleados.Show()
    End Sub

    Private Sub btnReportes_Click(sender As Object, e As EventArgs) Handles btnReportes.Click
        'FrmReportes.Show()
    End Sub

    Private Sub btnCerrarSesion_Click(sender As Object, e As EventArgs) Handles btnCerrarSesion.Click
        ModuloGlobal.NombreUsuario = ""
        ModuloGlobal.RolUsuario = ""
        Me.Hide()
        Login.Show()
    End Sub


End Class
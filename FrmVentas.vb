Imports System.Data.SqlClient

Public Class FrmVentas
    Dim conBD As Conexion = New Conexion()
    Private Sub FrmVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarClientes()
        CargarProductos()
        txtEmpleado.Text = ModuloGlobal.NombreUsuario

        dtpFecha.Value = DateTime.Now
    End Sub

    Private Sub CargarClientes()
        Dim consulta As String = "SELECT IdCliente, Nombre FROM Clientes"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        cboCliente.DataSource = tabla
        cboCliente.DisplayMember = "Nombre"
        cboCliente.ValueMember = "IdCliente"
        conBD.Cerrar()
    End Sub


    Private Sub CargarProductos()
        Dim consulta As String = "SELECT IdProducto, Nombre, Precio, Stock FROM Productos WHERE Stock > 0"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvProductos.DataSource = tabla
        conBD.Cerrar()
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If dgvProductos.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un producto de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not IsNumeric(txtCantidad.Text) OrElse CInt(txtCantidad.Text) <= 0 Then
            MessageBox.Show("Ingrese una cantidad válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim filaSeleccionada As DataGridViewRow = dgvProductos.CurrentRow
        Dim idProducto As Integer = Convert.ToInt32(filaSeleccionada.Cells("IdProducto").Value)
        Dim nombre As String = filaSeleccionada.Cells("Nombre").Value.ToString()
        Dim precio As Decimal = Convert.ToDecimal(filaSeleccionada.Cells("Precio").Value)
        Dim stock As Integer = Convert.ToInt32(filaSeleccionada.Cells("Stock").Value)
        Dim cantidad As Integer = Convert.ToInt32(txtCantidad.Text)

        If cantidad > stock Then
            MessageBox.Show("No hay suficiente stock disponible.", "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Verificar si ya está en el carrito
        For Each fila As DataGridViewRow In dgvDetalleVenta.Rows
            If Convert.ToInt32(fila.Cells("IdProducto").Value) = idProducto Then
                MessageBox.Show("Este producto ya fue agregado al carrito.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Next

        Dim subtotal As Decimal = cantidad * precio

        dgvDetalleVenta.Rows.Add(idProducto, nombre, precio, cantidad, subtotal)

        CalcularTotal()
        txtCantidad.Clear()
    End Sub

    Private Sub CalcularTotal()
        Dim total As Decimal = 0

        For Each fila As DataGridViewRow In dgvDetalleVenta.Rows
            total += Convert.ToDecimal(fila.Cells("Subtotal").Value)
        Next

        '' txtTotal.Text = total.ToString("F2")
    End Sub




End Class
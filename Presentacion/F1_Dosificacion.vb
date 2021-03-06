﻿Option Strict Off
Imports Logica.AccesoLogica
Imports DevComponents.DotNetBar
Imports Janus.Windows.GridEX
Imports System.IO
Imports DevComponents.DotNetBar.Controls

Public Class F1_Dosificacion

#Region "Variables Locales"

    Dim RutaGlobal As String = gs_CarpetaRaiz
    Dim RutaTemporal As String = "C:\Temporal"
    Dim Modificado As Boolean = False
    Dim nameImg As String = "Default.jpg"
    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem
#End Region
#Region "Metodos Privados"

    Private Sub _prIniciarTodo()
        L_prAbrirConexion(gs_Ip, gs_UsuarioSql, gs_ClaveSql, gs_NombreBD)
        _prCargarComboCompania(CbCompania)
        _prCargarComboAlmacen(CbAlmacen)

        _prCargarComboModulos(cbModulos)
        Me.Text = "DOSIFICACIÓN"
        Me.TbCodigo.ReadOnly = True

        _prAsignarPermisos()
        _PMIniciarTodo()

        'Dim blah As New Bitmap(New Bitmap(My.Resources.ic_f), 20, 20)
        'Dim ico As Icon = Icon.FromHandle(blah.GetHicon())
        'Me.Icon = ico

    End Sub
    Private Sub _prCargarComboModulos(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        Dim dt As New DataTable
        dt = L_fnListarModulos()
        With mCombo
            .DropDownList.Columns.Clear()
            .DropDownList.Columns.Add("cod").Width = 60
            .DropDownList.Columns("cod").Caption = "COD"
            .DropDownList.Columns.Add("desc").Width = 500
            .DropDownList.Columns("desc").Caption = "Modulo"
            .ValueMember = "cod"
            .DisplayMember = "desc"
            .DataSource = dt
            .Refresh()
        End With
    End Sub
    Private Sub _prCargarComboCompania(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        Dim dt As New DataTable
        dt = L_fnListarCompaniaDosificacion()
        With mCombo
            .DropDownList.Columns.Clear()
            .DropDownList.Columns.Add("cod").Width = 60
            .DropDownList.Columns("cod").Caption = "COD"
            .DropDownList.Columns.Add("desc").Width = 500
            .DropDownList.Columns("desc").Caption = "COMPAÑIA"
            .ValueMember = "cod"
            .DisplayMember = "desc"
            .DataSource = dt
            .Refresh()
        End With
    End Sub

    Private Sub _prCargarComboAlmacen(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        Dim dt As New DataTable
        dt = L_fnListarAlmacenDosificacion()
        With mCombo
            .DropDownList.Columns.Clear()
            .DropDownList.Columns.Add("cod").Width = 60
            .DropDownList.Columns("cod").Caption = "COD"
            .DropDownList.Columns.Add("desc").Width = 500
            .DropDownList.Columns("desc").Caption = "ALMACEN"
            .ValueMember = "cod"
            .DisplayMember = "desc"
            .DataSource = dt
            .Refresh()
        End With
    End Sub

    Public Sub _prStyleJanus()
        GroupPanelBuscador.Style.BackColor = Color.FromArgb(13, 71, 161)
        GroupPanelBuscador.Style.BackColor2 = Color.FromArgb(13, 71, 161)
        GroupPanelBuscador.Style.TextColor = Color.White
        JGrM_Buscador.RootTable.HeaderFormatStyle.FontBold = TriState.True
    End Sub

    Private Sub _prAsignarPermisos()
        'Dim dtRolUsu As DataTable = L_prRolDetalleGeneral(gi_userRol, _nameButton)

        'Dim show As Boolean = dtRolUsu.Rows(0).Item("ycshow")
        'Dim add As Boolean = dtRolUsu.Rows(0).Item("ycadd")
        'Dim modif As Boolean = dtRolUsu.Rows(0).Item("ycmod")
        'Dim del As Boolean = dtRolUsu.Rows(0).Item("ycdel")

        'If add = False Then
        '    btnNuevo.Visible = False
        'End If
        'If modif = False Then
        '    btnModificar.Visible = False
        'End If
        'If del = False Then
        '    btnEliminar.Visible = False
        'End If
    End Sub

#End Region

#Region "METODOS SOBRECARGADOS"

    Public Overrides Sub _PMOHabilitar()

        tbinicial.IsInputReadOnly = False
        tbfinal.IsInputReadOnly = False
        cbModulos.ReadOnly = False
        CbCompania.ReadOnly = False
        CbAlmacen.ReadOnly = False

        TbiSfc.IsInputReadOnly = False
        TbiNroFactura.IsInputReadOnly = False

        TbNroAutoriz.ReadOnly = False
        TbLlave.ReadOnly = False
        TbNota1.ReadOnly = False
        TbNota2.ReadOnly = False

        DtiFechaIni.IsInputReadOnly = False
        DtiFechaIni.ButtonDropDown.Enabled = True
        DtiFechaLim.IsInputReadOnly = False
        DtiFechaLim.ButtonDropDown.Enabled = True

        SbEstado.IsReadOnly = False

        CbCompania.Focus()
    End Sub

    Public Overrides Sub _PMOInhabilitar()
        CbCompania.ReadOnly = True
        CbAlmacen.ReadOnly = True
        cbModulos.ReadOnly = True
        tbinicial.IsInputReadOnly = True
        tbfinal.IsInputReadOnly = True

        TbiSfc.IsInputReadOnly = True
        TbiNroFactura.IsInputReadOnly = True

        TbNroAutoriz.ReadOnly = True
        TbLlave.ReadOnly = True
        TbNota1.ReadOnly = True
        TbNota2.ReadOnly = True

        DtiFechaIni.IsInputReadOnly = True
        DtiFechaIni.ButtonDropDown.Enabled = False
        DtiFechaLim.IsInputReadOnly = True
        DtiFechaLim.ButtonDropDown.Enabled = False

        SbEstado.IsReadOnly = True
        JGrM_Buscador.Focus()
        ' SuperTabItem1.Visible = False
    End Sub

    Public Overrides Sub _PMOLimpiar()
        If (CType(CbCompania.DataSource, DataTable).Rows.Count > 0) Then
            CbCompania.SelectedIndex = 0
        End If
        If (CType(CbAlmacen.DataSource, DataTable).Rows.Count > 0) Then
            CbAlmacen.SelectedIndex = 0
        End If
        If (CType(cbModulos.DataSource, DataTable).Rows.Count > 0) Then
            cbModulos.SelectedIndex = 0
        End If

        TbiSfc.Value = 0
        TbiNroFactura.Value = 0

        TbNroAutoriz.Clear()
        TbLlave.Clear()
        TbNota1.Clear()
        TbNota2.Clear()
        'cbModulos.Value = 1
        DtiFechaIni.Value = Now.Date
        DtiFechaLim.Value = Now.Date.AddMonths(6)

        SbEstado.Value = True
        swtipo.Value = True
        tbinicial.Value = 0
        tbfinal.Value = 0
    End Sub

    Public Overrides Sub _PMOLimpiarErrores()
        MEP.Clear()
        CbCompania.BackColor = Color.White
        CbAlmacen.BackColor = Color.White
        TbiSfc.BackColor = Color.White
        TbiNroFactura.BackColor = Color.White
        TbNroAutoriz.BackColor = Color.White
        TbLlave.BackColor = Color.White
        TbNota1.BackColor = Color.White
        TbNota2.BackColor = Color.White
        tbinicial.BackColor = Color.White
        tbfinal.BackColor = Color.White

    End Sub

    Public Overrides Function _PMOGrabarRegistro() As Boolean
        Dim res As Boolean = L_fnGrabarDosificacion(TbCodigo.Text.Trim,
                                                    CbCompania.Value.ToString,
                                                    CbAlmacen.Value.ToString,
                                                    TbiSfc.Value.ToString,
                                                    IIf(TbNroAutoriz.Text.Trim = String.Empty, 0, TbNroAutoriz.Text.Trim),
                                                    TbiNroFactura.Value.ToString,
                                                    TbLlave.Text.Trim,
                                                    DtiFechaIni.Value.ToString("yyyy/MM/dd"),
                                                    DtiFechaLim.Value.ToString("yyyy/MM/dd"),
                                                    TbNota1.Text.Trim,
                                                    TbNota2.Text.Trim,
                                                    IIf(SbEstado.Value, "1", "0"), IIf(swtipo.Value, 1, 0), tbinicial.Value, tbfinal.Value, cbModulos.Value)
        If res Then
            Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
            ToastNotification.Show(Me,
                                   "Código de dosificación ".ToUpper + TbCodigo.Text + " Grabado con Exito.".ToUpper,
                                   img,
                                   2000,
                                   eToastGlowColor.Green,
                                   eToastPosition.TopCenter)
            CbCompania.Focus()
        Else
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me,
                                   "La dosificación no pudo ser insertado".ToUpper,
                                   img,
                                   2000,
                                   eToastGlowColor.Red,
                                   eToastPosition.BottomCenter)
        End If
        Return res

    End Function

    Public Overrides Function _PMOModificarRegistro() As Boolean
        Dim res As Boolean

        res = L_fnModificarDosificacion(TbCodigo.Text.Trim,
                                        CbCompania.Value.ToString,
                                        CbAlmacen.Value.ToString,
                                        TbiSfc.Value.ToString, IIf(TbNroAutoriz.Text.Trim = String.Empty, 0, TbNroAutoriz.Text.Trim),
                                        TbiNroFactura.Value.ToString,
                                        TbLlave.Text.Trim,
                                        DtiFechaIni.Value.ToString("yyyy/MM/dd"),
                                        DtiFechaLim.Value.ToString("yyyy/MM/dd"),
                                        TbNota1.Text.Trim,
                                        TbNota2.Text.Trim,
                                        IIf(SbEstado.Value, "1", "0"), IIf(swtipo.Value, 1, 0), tbinicial.Value, tbfinal.Value, cbModulos.Value)
        If res Then
            Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
            ToastNotification.Show(Me, "Código de dosificación ".ToUpper + TbCodigo.Text + " modificado con Exito.".ToUpper,
                                   img,
                                   2000,
                                   eToastGlowColor.Green,
                                   eToastPosition.TopCenter)
        Else
            Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
            ToastNotification.Show(Me,
                                   "La dosificación no pudo ser modificado".ToUpper,
                                   img,
                                   2000,
                                   eToastGlowColor.Red,
                                   eToastPosition.BottomCenter)
        End If
        _PMInhabilitar()
        _PMPrimerRegistro()
        Return res
    End Function

    Public Overrides Sub _PMOEliminarRegistro()
        Dim ef = New Efecto

        ef.tipo = 2
        ef.Context = "¿esta seguro de eliminar el registro?".ToUpper
        ef.Header = "mensaje principal".ToUpper
        ef.ShowDialog()
        Dim bandera As Boolean = False
        bandera = ef.band
        If (bandera = True) Then
            Dim mensajeError As String = ""
            Dim res As Boolean = L_fnEliminarDosificacion(TbCodigo.Text, mensajeError,
                                        CbCompania.Value.ToString,
                                        CbAlmacen.Value.ToString,
                                        TbiSfc.Value.ToString,
                                        TbNroAutoriz.Text.Trim,
                                        TbiNroFactura.Value.ToString,
                                        TbLlave.Text.Trim,
                                        DtiFechaIni.Value.ToString("yyyy/MM/dd"),
                                        DtiFechaLim.Value.ToString("yyyy/MM/dd"),
                                        TbNota1.Text.Trim,
                                        TbNota2.Text.Trim,
                                        IIf(SbEstado.Value, "1", "0"))
            If res Then
                Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
                ToastNotification.Show(Me,
                                       "Código de dosificación ".ToUpper + TbCodigo.Text + " eliminado con Exito.".ToUpper,
                                       img,
                                       2000,
                                       eToastGlowColor.Green,
                                       eToastPosition.TopCenter)
                _PMFiltrar()
            Else
                Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                ToastNotification.Show(Me,
                                       mensajeError,
                                       img,
                                       2000,
                                       eToastGlowColor.Red,
                                       eToastPosition.BottomCenter)
            End If
        End If
    End Sub
    Public Overrides Function _PMOValidarCampos() As Boolean
        Dim _ok As Boolean = True
        MEP.Clear()

        If (Not IsNumeric(CbCompania.Value)) Then
            CbCompania.BackColor = Color.Red
            MEP.SetError(CbCompania, "Elija una compañia valida!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "Elija una compañia valida para efectuar la grabacion".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            CbCompania.BackColor = Color.White
            MEP.SetError(CbCompania, "")
        End If

        If (Not IsNumeric(CbAlmacen.Value)) Then
            CbAlmacen.BackColor = Color.Red
            MEP.SetError(CbAlmacen, "Elija un almacen valido!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "Elija una almacen valido para efectuar la grabacion".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            CbAlmacen.BackColor = Color.White
            MEP.SetError(CbAlmacen, "")
        End If

        If (TbiSfc.Value < 0) Then
            TbiSfc.BackColor = Color.Red
            MEP.SetError(TbiSfc, "el sfc debe ser mayor a cero!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "el sfc debe ser mayor a cero.".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            TbiSfc.BackColor = Color.White
            MEP.SetError(TbiSfc, "")
        End If

        If (TbNroAutoriz.Text = String.Empty And swtipo.Value = True) Then
            TbNroAutoriz.BackColor = Color.Red
            MEP.SetError(TbNroAutoriz, "el nro de autorización no puede quedar vacio!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "el nro de autorización no puede quedar vacio.".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            TbNroAutoriz.BackColor = Color.White
            MEP.SetError(TbNroAutoriz, "")
        End If

        If (TbLlave.Text = String.Empty And swtipo.Value = True) Then
            TbLlave.BackColor = Color.Red
            MEP.SetError(TbLlave, "la llave no puede quedar vacio!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "la llave no puede quedar vacio.".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            TbLlave.BackColor = Color.White
            MEP.SetError(TbLlave, "")
        End If

        If (TbNota1.Text = String.Empty) Then
            TbNota1.BackColor = Color.Red
            MEP.SetError(TbNota1, "la nota 1 no puede quedar vacio!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "la nota 1 no puede quedar vacio.".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            TbNota1.BackColor = Color.White
            MEP.SetError(TbNota1, "")
        End If

        If (TbNota2.Text = String.Empty) Then
            TbNota2.BackColor = Color.Red
            MEP.SetError(TbNota2, "la nota 2 no puede quedar vacio!".ToUpper)
            _ok = False
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "la nota 2 no puede quedar vacio.".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
        Else
            TbNota2.BackColor = Color.White
            MEP.SetError(TbNota2, "")
        End If
        If (swtipo.Value = False) Then
            If (tbinicial.ToString.Length <= 0) Then
                tbinicial.BackColor = Color.Red
                MEP.SetError(tbinicial, "El valor inicial de la factura no puede quedar vacio!".ToUpper)
                _ok = False
                Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
                ToastNotification.Show(Me, "El valor inicial de la dosificacion no puede quedar vacio".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)

            End If
            If (tbfinal.ToString.Length <= 0) Then
                tbfinal.BackColor = Color.Red
                MEP.SetError(tbfinal, "El valor inicial de la factura no puede quedar vacio!".ToUpper)
                _ok = False
                Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
                ToastNotification.Show(Me, "El valor Final de la dosificacion no puede quedar vacio".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)

            End If

        End If

            MHighlighterFocus.UpdateHighlights()
        Return _ok
    End Function

    Public Overrides Function _PMOGetTablaBuscador() As DataTable
        Dim dtBuscador As DataTable = L_fnGeneralDosificacion()
        Return dtBuscador
    End Function

    Public Overrides Function _PMOGetListEstructuraBuscador() As List(Of Modelos.Celda)
        Dim listEstCeldas As New List(Of Modelos.Celda)
        'a.aanumi ,a.aabdes ,a.aadir ,a.aatel ,a.aalat ,a.aalong ,a.aaimg,aata2dep ,a.aafact ,a.aahact ,a.aauact

        listEstCeldas.Add(New Modelos.Celda("numi", True, "Código".ToUpper, 80))
        listEstCeldas.Add(New Modelos.Celda("cia", True, "Compañia".ToUpper, 100))
        listEstCeldas.Add(New Modelos.Celda("alm", False, "Almacen".ToUpper, 100))
        listEstCeldas.Add(New Modelos.Celda("ALMACEN", True, "Almacen".ToUpper, 160))
        listEstCeldas.Add(New Modelos.Celda("sfc", True, "SFC".ToUpper, 80))
        listEstCeldas.Add(New Modelos.Celda("autoriz", True, "Nro Autorización".ToUpper, 150, "0"))
        listEstCeldas.Add(New Modelos.Celda("nfac", True, "Nro Factura".ToUpper, 120))
        listEstCeldas.Add(New Modelos.Celda("key", True, "Llave".ToUpper, 300))
        listEstCeldas.Add(New Modelos.Celda("fdel", True, "Fecha del".ToUpper, 100))
        listEstCeldas.Add(New Modelos.Celda("fal", True, "Fecha al".ToUpper, 100))
        listEstCeldas.Add(New Modelos.Celda("nota", True, "Nota 1".ToUpper, 300))
        listEstCeldas.Add(New Modelos.Celda("nota2", True, "Nota 2".ToUpper, 300))
        listEstCeldas.Add(New Modelos.Celda("est", True, "Estado".ToUpper, 80))
        listEstCeldas.Add(New Modelos.Celda("tipo", False))
        listEstCeldas.Add(New Modelos.Celda("inicio", False))
        listEstCeldas.Add(New Modelos.Celda("final", False))
        listEstCeldas.Add(New Modelos.Celda("sbmodulo", False))
        Return listEstCeldas
    End Function

    Public Overrides Sub _PMOMostrarRegistro(_N As Integer)
        JGrM_Buscador.Row = _MPos
        'a.aanumi ,a.aabdes ,a.aadir ,a.aatel ,a.aalat ,a.aalong ,a.aaimg,aata2dep ,a.aafact ,a.aahact ,a.aauact
        Dim dt As DataTable = CType(JGrM_Buscador.DataSource, DataTable)
        Try
            TbCodigo.Text = JGrM_Buscador.GetValue("numi").ToString
        Catch ex As Exception
            Exit Sub
        End Try
        With JGrM_Buscador
            TbCodigo.Text = .GetValue("numi").ToString
            CbCompania.SelectedIndex = .GetValue("cia") - 1
            CbAlmacen.SelectedIndex = .GetValue("alm") - 1
            TbiSfc.Value = .GetValue("sfc")
            TbNroAutoriz.Text = .GetValue("autoriz").ToString
            TbiNroFactura.Value = .GetValue("nfac")
            TbLlave.Text = .GetValue("key").ToString
            DtiFechaIni.Value = .GetValue("fdel")
            DtiFechaLim.Value = .GetValue("fal")
            cbModulos.Value = .GetValue("sbmodulo")
            TbNota1.Text = .GetValue("nota").ToString
            TbNota2.Text = .GetValue("nota2").ToString
            SbEstado.Value = .GetValue("est")
            swtipo.Value = .GetValue("tipo")
            tbinicial.Value = .GetValue("inicio")
            tbfinal.Value = .GetValue("final")
            'lbFecha.Text = CType(.GetValue("fact"), Date).ToString("dd/MM/yyyy")
            'lbHora.Text = .GetValue("hact").ToString
            'lbUsuario.Text = .GetValue("uact").ToString

        End With
        LblPaginacion.Text = Str(_MPos + 1) + "/" + JGrM_Buscador.RowCount.ToString
    End Sub

    Public Overrides Sub _PMOHabilitarFocus()
        'With MHighlighterFocus
        '    .SetHighlightOnFocus(tbCodigo, DevComponents.DotNetBar.Validator.eHighlightColor.Blue)
        'End With
    End Sub

#End Region

    Private Sub F1_Clientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _prIniciarTodo()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        If btnGrabar.Enabled = True Then
            _PMInhabilitar()
            _PMPrimerRegistro()

        Else
            '  Public _modulo As SideNavItem
            _modulo.Select()
            _tab.Close()
        End If
    End Sub

    Private Sub swtipo_ValueChanged(sender As Object, e As EventArgs) Handles swtipo.ValueChanged
        If (swtipo.Value = True) Then

            LabelFinal.Visible = False
            LbInicial.Visible = False
            tbinicial.Visible = False
            tbfinal.Visible = False
        Else
            LabelFinal.Visible = True
            LbInicial.Visible = True
            tbinicial.Visible = True
            tbfinal.Visible = True
        End If
    End Sub
End Class
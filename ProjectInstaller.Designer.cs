namespace EventMonitor
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServiceProcessInstallerEventMonitor = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceInstallerEventMonitor = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServiceProcessInstallerEventMonitor
            // 
            this.ServiceProcessInstallerEventMonitor.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.ServiceProcessInstallerEventMonitor.Password = null;
            this.ServiceProcessInstallerEventMonitor.Username = null;
            // 
            // ServiceInstallerEventMonitor
            // 
            this.ServiceInstallerEventMonitor.ServiceName = "EventMonitor";
            this.ServiceInstallerEventMonitor.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.ServiceInstallerEventMonitor.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServiceProcessInstallerEventMonitor,
            this.ServiceInstallerEventMonitor});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServiceProcessInstallerEventMonitor;
        private System.ServiceProcess.ServiceInstaller ServiceInstallerEventMonitor;
    }
}
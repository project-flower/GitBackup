# Introduction
Git でコミット前のファイルをバックアップします。

# Build and Test
GitBackup.sln から GitBackup.csproj をビルドして下さい。

# About App.config
user.config (ユーザースコープ)は扱いづらいので、GitBackup.exe.config (アプリケーションスコープ)で設定を変更できるようになっています。

ビルドすると GitBackup.exe と同じ場所に GitBackup.exe.config が作成されるので、
実行する環境に応じて設定を変更して下さい。
`Branches` はブランチの一覧(StringCollection)を、
`Command` は Git の diff コマンドを(通常は変更の必要なし)、
`Destinations` はバックアップ先の一覧(StringCollection)を、
`GitBin` は git.exe のパスを、
`NewLine` は git.exe が出力する改行コード(通常は変更の必要なし)を
設定します。

## 設定例

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" >
            <section name="GitBackup.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
    <applicationSettings>
        <GitBackup.Properties.Settings>
            <setting name="Branches" serializeAs="Xml">
                <value>
                    <ArrayOfString xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                        xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                        <string>C:\source\repos\GitBackup</string>
                        <string>C:\source\repos\pffs</string>
                    </ArrayOfString>
                </value>
            </setting>
            <setting name="Command" serializeAs="String">
                <value>diff --diff-filter=ACMRT --name-only HEAD</value>
            </setting>
            <setting name="Destinations" serializeAs="Xml">
                <value>
                    <ArrayOfString xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                        xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                        <string>C:\Documents\GitBackup</string>
                    </ArrayOfString>
                </value>
            </setting>
            <setting name="GitBin" serializeAs="String">
                <value>C:\Program Files\Git\bin\git.exe</value>
            </setting>
            <setting name="NewLine" serializeAs="String">
                <value>\n</value>
            </setting>
        </GitBackup.Properties.Settings>
    </applicationSettings>
</configuration>
```
#include "common.hpp"
#include "Downloadfile.hpp"

QString transformGitHubURL(QString URLBase)
{
    DownloadFile *a = new DownloadFile(QUrl(URLBase));

    return "";
}

QString transformURL(QString URLBase)
{
    if(URLBase.contains("github.com"))
        return transformGitHubURL(URLBase);
    return "";
}

QString getDownloadLink(QString path)
{
    QFile file;
    QString downloadLink;

    file.setFileName(path);
    file.open(QIODevice::ReadOnly | QIODevice::Text);

    downloadLink = QJsonDocument::fromJson(file.readAll()).object().value("DOWNLOAD").toString();
    file.close();

    return transformURL(downloadLink);
}

#include "common.hpp"
#include "Downloadfile.hpp"

DownloadFile::DownloadFile(QUrl URL) : QWidget()
{
    QNetworkRequest request(URL);
    QNetworkAccessManager manager(this);
    this->reply = manager.get(request);

    connect(reply, SIGNAL(reply.finished()), this, SLOT(dataDownload()));
}

QByteArray DownloadFile::dataDownload()
{
    string tmp = this->reply->readAll().toStdString();
    cout << "Ca marche !" << endl;

    return this->reply->readAll();
}

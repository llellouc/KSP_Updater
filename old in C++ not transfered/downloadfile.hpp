#ifndef DOWNLOADFILE_HPP
#define DOWNLOADFILE_HPP

#include "common.hpp"
#include <QWidget>

class DownloadFile : public QWidget
{
    Q_OBJECT

public :
    DownloadFile(QUrl URL);

private:
    QNetworkReply *reply;

public slots :
    QByteArray dataDownload();
};

#endif // DOWNLOADFILE_HPP

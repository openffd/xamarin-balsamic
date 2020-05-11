KEY=""
SECRET=""
BUCKET=""

function upload
{
  path=$1
  file=$2
  remote_path=$3
  date=$(date +"%a, %d %b %Y %T %z")
  acl="x-amz-acl:public-read"
  string="PUT\n\napplication/x-compressed-tar\n$date\nx-amz-acl:public-read\n/${BUCKET}$remote_path$file"
  sig=$(echo -en "${string}" | openssl sha1 -hmac "${SECRET}" -binary | base64)
  curl -X PUT -T "$path/$file" \
    -H "Host: ${BUCKET}.s3.amazonaws.com" \
    -H "Date: $data" \
    -H "Content-Type: $content_type" \
    -H "$acl" \
    -H "Authorization: AWS ${KEY}:$sig" \
    "https://$bucket.s3.amazonaws.com$remote_path$file"
}



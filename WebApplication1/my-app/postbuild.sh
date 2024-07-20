if [ -d "./../ui" ]; then
    rm -rv ./../ui/*
    mv -v build/* ./../ui/
else
    mkdir -p ./../ui
    mv -v build/* ./../ui/
fi
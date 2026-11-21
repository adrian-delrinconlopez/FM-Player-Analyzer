add_executable(fmbench bench_cli.cpp)
target_link_libraries(fmbench PRIVATE fmcore)

add_executable(fmprofilebench profilebench_cli.cpp)
target_link_libraries(fmprofilebench PRIVATE fmcore)

add_executable(fm27savedump fm27savedump_cli.cpp)
target_link_libraries(fm27savedump PRIVATE fmcore)

add_executable(fmratingcheck ratingcheck_cli.cpp)
target_link_libraries(fmratingcheck PRIVATE fmcore)

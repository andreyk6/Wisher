'use strict';
var gulp = require('gulp'),
	mainBowerFiles = require('main-bower-files'),
	jade = require('gulp-jade'),
	connect = require('gulp-connect'),
	sass = require('gulp-ruby-sass'),
	postcss = require('gulp-postcss'),
	sourcemaps = require('gulp-sourcemaps'),
	autoprefixer = require('autoprefixer'),
	mqpacker = require("css-mqpacker"),
	plumber = require('gulp-plumber'),
	imagemin = require('gulp-imagemin'),
	pngquant = require('imagemin-pngquant'),
	gulpif = require('gulp-if'),
	spritesmith = require('gulp.spritesmith'),
	uglify = require('gulp-uglify'),
	base64 = require('gulp-base64'),
	uglifycss = require('gulp-uglifycss'),
	dirs = {
		'source': {
			'vendorJs': './source/js/vendor/',
			'js': './source/js/**/*.js',
			'fonts': './source/fonts/**/*',
			'jade': './source/views/*.jade',
			'jadeBlocks': ['./source/views/blocks/*.jade', './source/views/layout/*.jade'],
			'jade_watch': './source/views/**/*.jade',
			'sass': './source/sass/**/*.sass',
			'sassFolder': './source/sass/',
			'img': './source/img/*.*',
			'icons': './source/img/icons/*.png'
		},
		'build': {
			'vendorJs': './build/js/vendor/',
			'vendorCss': './build/css/vendor/',
			'jadeBloks': './build/jade-blocks/',
			'css': './build/css/',
			'js': './build/js/',
			'fonts': './build/fonts/',
			'build': './build',
			'img': './build/img/'
		}
	};

gulp.task('vendor-js', function() {
	return gulp.src(mainBowerFiles('**/*.js'))
	.pipe(plumber())
	.pipe(gulp.dest(dirs.source.vendorJs));
});

gulp.task('vendor-css', function() {
	return gulp.src(mainBowerFiles('**/*.css'))
	.pipe(plumber())
	.pipe(uglifycss())
	.pipe(gulp.dest(dirs.build.vendorCss));
});

gulp.task('fonts', function() {
	gulp.src(dirs.source.fonts)
	.pipe(gulp.dest(dirs.build.fonts));
});

gulp.task('connect', function() {
	connect.server({
		root: dirs.build.build,
		livereload: true,
		port: 8888
	});
});

//jade
gulp.task('templates', function() {
	var YOUR_LOCALS = {};

	gulp.src(dirs.source.jade)
	.pipe(plumber())
	.pipe(jade({
		pretty: true,
		locals: YOUR_LOCALS
	}))
	.pipe(gulp.dest(dirs.build.build))
	.pipe(connect.reload());
});

//jade
gulp.task('templates-blocks', function() {
	var YOUR_LOCALS = {};

	gulp.src(dirs.source.jadeBlocks)
	.pipe(plumber())
	.pipe(jade({
		pretty: true,
		locals: YOUR_LOCALS
	}))
	.pipe(gulp.dest(dirs.build.jadeBloks))
	.pipe(connect.reload());
});

//sass
gulp.task('sass', function() {

	var processors = [
		require('postcss-opacity'),
		autoprefixer({browsers: ['last 2 version', 'IE 8', 'IE 9', 'IE 10', 'IE 11', 'Opera 12'], cascade: false}),
		mqpacker({
			sort: function (a, b) {
				a = a.replace(/\D/g,'');
				b = b.replace(/\D/g,'');
				return b-a;
			}
		})
	];

	return sass(dirs.source.sass, {
		sourcemap: true,
		style: 'compact'
	})
	.on('error', function (err) {
		console.error('Error', err.message);
	})
	.pipe(base64({
		exclude: ['icon']
	}))
	.pipe(postcss(processors))
	.pipe(sourcemaps.write('./'))
	.pipe(gulp.dest(dirs.build.css))
	.pipe(connect.reload());
});

//images
gulp.task('images', function() {
	return gulp.src(dirs.source.img)
		.pipe(plumber())
		.pipe(gulpif(/[.](png|jpeg|jpg|svg)$/, imagemin({
			progressive: true,
			svgoPlugins: [{
				removeViewBox: false
			}],
			use: [pngquant()]
		})))
		.pipe(gulp.dest(dirs.build.img))
		.pipe(connect.reload());
});

// sprite
gulp.task('sprite', function() {
	var spriteData = gulp.src(dirs.source.icons)
	.pipe(plumber())
	.pipe(spritesmith({
		imgName: 'icons.png',
		cssName: '_sprite.sass',
		imgPath: '../img/icons.png',
		cssFormat: 'sass',
		padding: 4,
		// algorithm: 'top-down',
		cssTemplate: 'source/helpers/sprite.template.mustache'
	}));
	spriteData.img
		.pipe(gulp.dest(dirs.build.img));
	spriteData.css
		.pipe(gulp.dest(dirs.source.sassFolder));
});

gulp.task('js', function() {
	return gulp.src(dirs.source.js)
	.pipe(plumber())
	.pipe(uglify())
	.pipe(gulp.dest(dirs.build.js))
	.pipe(connect.reload());
});

gulp.task('watch', function(){
	gulp.watch(dirs.source.jade_watch, ['templates']);
	gulp.watch(dirs.source.sass, ['sass']);
	gulp.watch(dirs.source.js, ['js']);
	gulp.watch(dirs.source.img, ['images']);
	gulp.watch(dirs.source.icons, ['sprite']);
});



gulp.task('default', ['fonts', 'vendor-js', 'vendor-css', 'js', 'sprite', 'images', 'templates', 'sass', 'connect', 'watch']);
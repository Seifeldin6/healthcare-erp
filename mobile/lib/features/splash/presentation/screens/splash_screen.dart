import 'package:flutter/material.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/routing/routes.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';

import '../widgets/splash_footer.dart';
import '../widgets/splash_logo.dart';
import '../widgets/splash_texts.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen> {
  @override
  void initState() {
    super.initState();

    WidgetsBinding.instance.addPostFrameCallback((_) {
      FlutterNativeSplash.remove();
    });

    _start();
  }


  Future<void> _start() async {
    await Future.delayed(const Duration(milliseconds: 4000));
    if (!mounted) return;
    Navigator.pushReplacementNamed(context, Routes.onBoardingScreen);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.primary,
      body: Stack(
        alignment: Alignment.center,
        children: [
          const SplashLogo(),
          Positioned(top: 500.h, child: const SplashTexts()),
          Positioned(bottom: 72.h, child: const SplashFooter()),
        ],
      ),
    );
  }
}

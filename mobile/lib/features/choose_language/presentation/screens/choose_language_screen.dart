import 'package:flutter/material.dart';
import 'package:healthcare_app/core/routing/routes.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/features/choose_language/presentation/widgets/choose_language_header.dart';
import 'package:healthcare_app/features/choose_language/presentation/widgets/choose_language_sheet.dart';

class ChooseLanguageScreen extends StatelessWidget {
  const ChooseLanguageScreen({super.key});

  void _onLanguageChosen(BuildContext context, String languageCode) {
    Navigator.pushReplacementNamed(context, Routes.onBoardingScreen);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.primary,
      body: Column(
        children: [
          const Expanded(
            child: ChooseLanguageHeader(),
          ),
          ChooseLanguageSheet(
            onSelectArabic: () => _onLanguageChosen(context, 'ar'),
            onSelectEnglish: () => _onLanguageChosen(context, 'en'),
          ),
        ],
      ),
    );
  }
}
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/consultation/chat_video_mockup_card.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/onboarding_page_content.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/pharmacists/pharmacists_mockup_card.dart';
import 'package:healthcare_app/features/onboarding/presentation/widgets/prescription/prescription_summary_mockup_card.dart';
import 'package:healthcare_app/generated/l10n.dart';
import '../widgets/onboarding_top_bar.dart';

class OnboardingScreen extends StatefulWidget {
  const OnboardingScreen({super.key});

  @override
  State<OnboardingScreen> createState() => _OnboardingScreenState();
}

class _OnboardingScreenState extends State<OnboardingScreen> {
  final PageController _pageController = PageController();
  int _currentIndex = 0;

  @override
  void dispose() {
    _pageController.dispose();
    super.dispose();
  }

  void _onBack() {
    if (_currentIndex > 0) {
      _pageController.previousPage(
        duration: const Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    }
  }

  void _onSkip() {}

  void _onNext() {
    if (_currentIndex < 2) {
      _pageController.nextPage(
        duration: const Duration(milliseconds: 300),
        curve: Curves.easeInOut,
      );
    } else {}
  }

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return AnnotatedRegion<SystemUiOverlayStyle>(
      value: SystemUiOverlayStyle.dark,
      child: Scaffold(
        backgroundColor: AppColors.surface,
        body: SafeArea(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: AppDimensions.spacing12.h),
              Padding(
                padding: EdgeInsetsDirectional.symmetric(
                  horizontal: AppDimensions.spacing24.w,
                ),
                child: OnboardingTopBar(
                  currentIndex: _currentIndex,
                  onBack: _onBack,
                  onSkip: _onSkip,
                ),
              ),
              Expanded(
                child: PageView(
                  controller: _pageController,
                  onPageChanged: (index) {
                    setState(() {
                      _currentIndex = index;
                    });
                  },
                  children: [
                    OnboardingPageContent(
                      mockupCard: const PharmacistsMockupCard(),
                      currentIndex: _currentIndex,
                      title: s.onboardingStep1Title,
                      description: s.onboardingStep1Description,
                    ),
                    OnboardingPageContent(
                      mockupCard: const ChatVideoMockupCard(),
                      currentIndex: _currentIndex,
                      title: s.onboardingStep2Title,
                      description: s.onboardingStep2Description,
                    ),
                    OnboardingPageContent(
                      mockupCard: const PrescriptionSummaryMockupCard(),
                      currentIndex: _currentIndex,
                      title: s.onboardingStep3Title,
                      description: s.onboardingStep3Description,
                    ),
                  ],
                ),
              ),
              Padding(
                padding: EdgeInsetsDirectional.symmetric(
                  horizontal: AppDimensions.spacing24.w,
                ),
                child: AppPrimaryButton(
                  text: _currentIndex == 2
                      ? s.onboardingCreateAccount
                      : s.onboardingNext,
                  suffixIcon: _currentIndex == 2
                      ? null
                      : Icons.arrow_forward_rounded,
                  onPressed: _onNext,
                ),
              ),
              SizedBox(height: AppDimensions.spacing24.h),
            ],
          ),
        ),
      ),
    );
  }
}